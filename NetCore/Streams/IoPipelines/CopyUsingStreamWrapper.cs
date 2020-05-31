using System;
using System.IO;
using System.Threading.Tasks;

namespace IoPipelines
{
    public class CopyUsingStreamWrapper
    {
        public async Task Copy()
        {
            await using var stream = new StreamWrapper();
            stream.StartWriting(CopyFrom);
            stream.StartReading(CopyTo);
            
            Console.WriteLine("Copy in between");
        }

        private async Task CopyFrom(Stream stream)
        {
            Console.WriteLine("Starting Read!");
            var source = new FileStream("C:\\temp\\LibreOffice_4.2.0_Win_x86.msi", FileMode.Open);
            await source.CopyToAsync(stream);
        }

        private async Task CopyTo(Stream stream)
        {            
            Console.WriteLine("Starting Write!");
            var destination = new FileStream("D:\\temp\\target.msi", FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(destination);
        }
    }
}