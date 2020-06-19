using System.Numerics;

namespace RayTracingInOneWeekend {

    public abstract class Texture
    {
        public abstract Vector3 value(float u, float v, in Vector3 point);
    }

    public class SolidColor : Texture
    {
        public SolidColor(in Vector3 color)
        {
            this.color = color;
        }

        public SolidColor(float value)
        {
            this.color = new Vector3(value);
        }
        public SolidColor(float r, float g, float b)
        {
            this.color = new Vector3(r, g, b);
        }

        public override Vector3 value(float u, float v, in Vector3 point)
        {
            return color;
        }

        private readonly Vector3 color;
    }

    public class CheckerTexture : Texture
    {
        public CheckerTexture(in Texture odd, in Texture even)
        {
            this.odd = odd;
            this.even = even;
        }

        public override Vector3 value(float u, float v, in Vector3 point)
        {
            float sine = Utility.SinF(10.0f * point.X) * Utility.SinF(10.0f * point.Y) * Utility.SinF(10.0f * point.Z);
            return sine < 0.0f ? odd.value(u, v, point) : even.value(u, v, point);
        }

        private readonly Texture odd;
        private readonly Texture even;
    }
}

