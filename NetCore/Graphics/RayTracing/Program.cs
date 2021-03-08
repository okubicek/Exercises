using System;
using RayTracing.Utils;

namespace RayTracing
{
    public class Program
    {
        static void Main(string[] args)
        {
            var aspectRatio = 16.0 / 9.0;
            var imageWidth = 400;
            var imageHeight = (int) (imageWidth / aspectRatio);

            var allHittableObjects = new HittableList();
            allHittableObjects.Add(new Sphere(new Point(0,0,-1), 0.5));
            allHittableObjects.Add(new Sphere(new Point(0,-100.5,-1), 100));

            var viewPortHeight = 2.0;
            var viewPortWidth = viewPortHeight * aspectRatio;
            var focalLength = 1.0;

            var origin = new Point(0, 0, 0);
            var horizontal = new Vector(viewPortWidth, 0, 0);
            var vertical = new Vector(0, viewPortHeight, 0);
            var bottomLeftCorner = origin - horizontal / 2 - vertical / 2 - new Vector(0,0, focalLength);

            var ppmHeader = new PpmHeader(imageWidth, imageHeight);

            Console.Write(ppmHeader.ToString());
            var consoleOutput = Console.Error;
            for (int j = imageHeight-1; j >= 0; j--)
            {
                consoleOutput.WriteLine($"Remaining {j}");
                for (int i = 0; i < imageWidth; i++)
                {
                    var u = (double)i/(imageWidth-1);
                    var v = (double)j/(imageHeight-1);

                    var ray = new Ray(origin, bottomLeftCorner + u * horizontal + v * vertical - origin);
                    var pixelColor = RayColor(ray, allHittableObjects);
                    ColorUtils.WriteColor(pixelColor);                    
                } 
            }
        }

        public static Vector RayColor(Ray ray, IHittable world)        
        {            
            var (isHit, rec) = world.Hit(ray, 0, double.MaxValue);
            if (isHit) return 0.5 * (rec.Normal + new Color(1,1,1));

            var unitDirection = Vector.UnitVector(ray.Direction);
            var t = 0.5 * (unitDirection.Y + 1.0);

            return (1.0 - t) * new Color(1.0, 1.0, 1.0) + t * new Color(0.5, 0.7, 1.0);
        }

        public static double HitSphere(Point center, double radius, Ray ray)
        {
            var oc = ray.Origin - center;
            var a = ray.Direction.LengthSquared();
            var halfB = Vector.Dot(oc, ray.Direction);
            var c = oc.LengthSquared() - radius * radius;
            var discriminant = halfB * halfB - a * c;

            return discriminant < 0 ? -1.0 : (-halfB - Math.Sqrt(discriminant)) / (a);
        }
    }
}