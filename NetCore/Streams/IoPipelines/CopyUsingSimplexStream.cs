using System;
using System.IO;
using System.Threading.Tasks;
using Nerdbank.Streams;

namespace IoPipelines
{
    public class CopyUsingSimplexStream
    {
        public async Task Copy()
        {
            var sourceStream = new SimplexStream();
            var tc = CopyFrom(sourceStream);
                
            var targetStream = new SimplexStream();
            var td = CopyTo(targetStream);

            Console.WriteLine("Copy in between");

            await sourceStream.CopyToAsync(targetStream);
            targetStream.CompleteWriting();

            await Task.WhenAll(tc, td);
        }

        private Task CopyFrom(SimplexStream stream)
        {
            var task = Task.Run(async delegate {
                Console.WriteLine("Starting Read!");
                var source = new FileStream("C:\\temp\\LibreOffice_4.2.0_Win_x86.msi", FileMode.Open);
                await source.CopyToAsync(stream);
                stream.CompleteWriting();
            });

            return task;
        }

        private Task CopyTo(SimplexStream stream)
        {
            var task = Task.Run(async delegate {
                Console.WriteLine("Starting Write!");
                var destination = new FileStream("D:\\temp\\target.msi", FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(destination);
            });

            return task;
        }
    }
}