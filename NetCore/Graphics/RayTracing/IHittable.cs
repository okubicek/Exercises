namespace RayTracing 
{
    public interface IHittable
    {
        (bool isHit, HitRecord hitDetail) Hit(Ray ray, double tmin, double tmax);
    }
}