using System.Text;

namespace RayTracing
{
    public class Vector
    {
        private double[] _vec;

        public Vector()
        {
            _vec = new double[] { 0, 0, 0 };
        }

        public Vector(double x, double y, double z)
        {
            _vec = new double[] { x, y, z };
        }

        public double X => _vec[0];
        public double Y => _vec[1];
        public double Z => _vec[2];

        public static Vector operator -(Vector a) => new Vector(-a.X, -a.Y, -a.Z);

        public static Vector operator -(Vector a, Vector b) => new Vector(a.X -b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector operator *(Vector a, double t) => new Vector(a.X * t, a.Y * t, a.Z * t);

        public static Vector operator *(double t, Vector a) => a * t;

        public static Vector operator *(Vector a, Vector b) => new Vector(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        public static Vector operator /(Vector a, double t) => new Vector(a.X / t, a.Y / t, a.Z / t);

        public double LengthSquared() => X * X + Y * Y + Z * Z;

        public double Length() => (double) System.Math.Sqrt(LengthSquared());

        public static double Dot(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public Vector Cross(Vector a, Vector b)
        {
            return new Vector(a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        public static Vector UnitVector(Vector v)
        {
            return v / v.Length();
        }

        public double this[int i]
        {
            get { return _vec[i]; }
        }
    }
}

