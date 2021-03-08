using System;
using RayTracing;

namespace RayTracing.Utils
{
    public class ColorUtils
    {
        public static void WriteColor(Vector c)
        {
            Console.WriteLine($"{(int)(255.999 * c.X)} {(int)(255.999 * c.Y)} {(int)(255.999 * c.Z)}");
        }
    }
}