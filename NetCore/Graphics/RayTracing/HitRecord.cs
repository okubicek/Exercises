namespace RayTracing 
{
    public struct HitRecord
    {
        public Point Point{ get; set; }

        public Vector Normal { get; set; }

        public double T { get; set; }

        public bool FrontFace { get; private set; }

        public void SetFaceNormal(Ray ray, Vector outwardNormal)
		{
            FrontFace = Vector.Dot(ray.Direction, outwardNormal) < 0;
            Normal = FrontFace ? outwardNormal : -outwardNormal;
		}
    }
}