using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Nerdbank.Streams;

namespace IoPipelines
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var copier = new SiplexStreamImplementation();
            var copier = new CopyUsingStreamWrapper();
            await copier.Copy();
        }        
    }
}
