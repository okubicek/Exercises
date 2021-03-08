namespace RayTracing
{
    public class Ray
    {
        public Vector Origin { get; }

        public Vector Direction { get; }

        public Ray(Vector origin, Vector direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector At(double t)
        {
            return Origin + t * Direction;
        }
    }
}