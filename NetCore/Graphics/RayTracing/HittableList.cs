using System.Collections.Generic;

namespace RayTracing
{
	public class HittableList : List<IHittable>, IHittable
	{
		public (bool, HitRecord) Hit(Ray ray, double tmin, double tmax)
		{			
			bool anyHits = false;
			var closestOne = tmax;
			HitRecord result = new HitRecord();
			foreach (var hittable in this)
			{
				var (hit, temp) = hittable.Hit(ray, tmin, closestOne);
				if (hit)
				{
					anyHits = true;
					closestOne = temp.T;
					result = temp;
				}
			}

			return (anyHits, result);
		}
	}
}
