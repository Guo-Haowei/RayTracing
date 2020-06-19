using System;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RayTracingInOneWeekend
{
    class Program
    {
        static Vector3 rayColor(in Ray ray, in HittableList world)
        {
            HitRecord hitRecord = new HitRecord();
            if (world.hit(ray, 0.0f, float.PositiveInfinity, ref hitRecord))
            {
                return 0.5f * (hitRecord.normal + Vector3.One);
            }

            Vector3 unitDirection = Vector3.Normalize(ray.direction);
            float t = 0.5f * unitDirection.Y + 0.5f;
            Vector3 white = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 blue = new Vector3(0.5f, 0.7f, 1.0f);

            return (1.0f - t) * white + t * blue;
        }

        static void Main()
        {
            const float aspectRatio = 16.0f / 9.0f;
            const int imageWidth = 384;
            const int imageHeight = (int)(imageWidth / aspectRatio);
            const int samplesPerPixel = 100;

            const int component = 3;
            const int stride = component * imageWidth;

            byte[] imageBuffer = new byte[stride * imageHeight];

            HittableList world = new HittableList();
            world.add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f));
            world.add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f));

            Camera camera = new Camera(aspectRatio);

            for (int index = 0; index < imageWidth * imageHeight; ++index)
            {
                int i = index % imageWidth;
                int j = imageHeight - (index / imageWidth + 1);

                Vector3 color = Vector3.Zero;
                for (int s = 0; s < samplesPerPixel; ++s)
                {
                    float u = (i + Utility.RandomF()) / (imageWidth - 1);
                    float v = (j + Utility.RandomF()) / (imageHeight - 1);
                    Ray ray = camera.getRay(u, v);
                    color += rayColor(ray, world);
                }

                color *= (1.0f / samplesPerPixel);
                byte br = (byte)(255.999f * color.X);
                byte bg = (byte)(255.999f * color.Y);
                byte bb = (byte)(255.999f * color.Z);
                imageBuffer[3 * index] = bb;
                imageBuffer[3 * index + 1] = bg;
                imageBuffer[3 * index + 2] = br;
            }

            Bitmap bitmap = new Bitmap(imageWidth, imageHeight, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(imageBuffer, 0));
            bitmap.Save("../image.png", ImageFormat.Png);
            bitmap.Dispose();
        }
    }
}
