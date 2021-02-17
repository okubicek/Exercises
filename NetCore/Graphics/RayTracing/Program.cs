using System;

namespace RayTracing
{
    class Program
    {
        static void Main(string[] args)
        {
            var (imageWidth, imageHeight) = (256,256);
            var ppmHeader = new PpmHeader(imageWidth, imageHeight);

            Console.Write(ppmHeader.ToString());
            var consoleOutput = Console.Error;
            for (int j = imageHeight-1; j >= 0; j--)
            {
                consoleOutput.WriteLine($"Remaining {j}");
                for (int i = 0; i < imageWidth; i++)
                {
                    float r = i / ((float) imageWidth - 1);
                    float g = j / ((float) imageHeight - 1);
                    float b = 0.25f;

                    int ir = (int)(255.999 * r);
                    int ig = (int)(255.999 * g);
                    int ib = (int)(255.999 * b);

                    Console.WriteLine($"{ir} {ig} {ib}");
                } 
            }
        }
    }
}


