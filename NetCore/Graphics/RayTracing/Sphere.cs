using System;

namespace RayTracing 
{
    public class Sphere : IHittable
    {
        private Point _center;

        private double _radius;

		public Sphere(Point center, double radius)
		{
			_center = center;
			_radius = radius;
		}

		public (bool isHit, HitRecord hitDetail) Hit(Ray ray, double tmin, double tmax)
		{
			var oc = ray.Origin - _center;
			var a = ray.Direction.LengthSquared();
			var halfB = Vector.Dot(oc, ray.Direction);
			var c = oc.LengthSquared() - _radius * _radius;

			var discriminant = halfB * halfB - a * c;
			if (discriminant < 0) return (false, new HitRecord());

			var sqrtd = Math.Sqrt(discriminant);

			var root = (-halfB - sqrtd) / a;

			if (root < tmin || tmax < root)
			{
				root = (-halfB + sqrtd) / a;
				if (root < tmin || tmax < root)
				{
					return (false, new HitRecord());
				}
			}

			var hit = new HitRecord();
			hit.T = root;
			hit = GetHitPoint(ray, hit);
			var outwardNormal = (hit.Point - _center) / _radius;
			hit.SetFaceNormal(ray, outwardNormal);

			return (true, hit);
		}

		private static HitRecord GetHitPoint(Ray ray, HitRecord hit)
		{
			var r = ray.At(hit.T);
			hit.Point = new Point(r.X, r.Y, r.Z);
			return hit;
		}
	}
}