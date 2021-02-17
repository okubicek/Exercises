using System.Text;

namespace RayTracing{
    public class PpmHeader
    {
        public PpmHeader(int width, int height)
        {
            Width = width;
            Height = height;
        }

        private const string ColorType = "P3";

        private const int MaxColor = 255;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine(ColorType);
            builder.AppendLine($"{Width} {Height}");
            builder.AppendLine($"{MaxColor}");

            return builder.ToString();
        }
    }
}