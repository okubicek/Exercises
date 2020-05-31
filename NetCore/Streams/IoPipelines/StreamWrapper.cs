using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nerdbank.Streams;

namespace IoPipelines
{
    public class StreamWrapper : Stream, IAsyncDisposable
    {
        private SimplexStream _stream = new SimplexStream();

        private List<Task> _tasksToAwait = new List<Task>();
        
        public void StartWriting(Func<Stream, Task> copyTo)
        {
            var task = Task.Run(async delegate {                
                await copyTo(_stream);            
                _stream.CompleteWriting();
            });

            _tasksToAwait.Add(task);
        }

        public void StartReading(Func<Stream, Task> copyFrom)
        {
            var task = Task.Run(async delegate {                
                await copyFrom(_stream);
            });

            _tasksToAwait.Add(task);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_tasksToAwait.Any())
            {
                await Task.WhenAll(_tasksToAwait);
            }
        }

        public override void Flush() => _stream.Flush();
        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);        
        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);
        public override void SetLength(long value) => _stream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => Write(buffer, offset, count);
        
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
            _stream.WriteAsync(buffer, offset, count, cancellationToken);

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
            _stream.ReadAsync(buffer, offset, count, cancellationToken);

        public void CompleteWriting()
        {
            _stream.CompleteWriting();
        }

        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            await _stream.CopyToAsync(destination, bufferSize, cancellationToken);

            if (destination is StreamWrapper)
            {
                ((StreamWrapper)destination).CompleteWriting();
            }
        }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;
        public override long Position { get => _stream.Position; set => _stream.Position = value; }
    }
}