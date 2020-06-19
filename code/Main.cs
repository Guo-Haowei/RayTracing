using System;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RayTracingInOneWeekend
{
    class Program
    {
        static Vector3 rayColor(in Ray ray, in HittableList world, int depth)
        {
            if (depth <= 0)
                return Vector3.Zero;

            HitRecord hitRecord = new HitRecord();
            if (world.hit(ray, 0.001f, float.PositiveInfinity, ref hitRecord))
            {
                Ray scattered = new Ray();
                Vector3 attenuation = new Vector3();
                if (hitRecord.material.scatter(ray, hitRecord, ref attenuation, ref scattered))
                {
                    return attenuation * rayColor(scattered, world, depth - 1);
                }

                return Vector3.Zero;
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
            const int maxDepth = 50;

            const int component = 3;
            const int stride = component * imageWidth;

            byte[] imageBuffer = new byte[stride * imageHeight];

            HittableList world = new HittableList();
            world.add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, new Lambertian(new Vector3(0.1f, 0.2f, 0.5f))));
            world.add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, new Lambertian(new Vector3(0.8f, 0.8f, 0.0f))));
            world.add(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, new Metal(new Vector3(0.8f, 0.6f, 0.2f), 0.3f)));
            world.add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, new Dielectric(1.5f)));
            world.add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), -0.45f, new Dielectric(1.5f)));

            Vector3 lookFrom = new Vector3(3.0f, 3.0f, 2.0f);
            Vector3 lookAt = new Vector3(0.0f, 0.0f, -1.0f);
            float focusDistance = Vector3.Distance(lookFrom, lookAt);
            float aperture = 1.0f;
            Camera camera = new Camera(
                lookFrom,
                lookAt,
                Vector3.UnitY,
                20.0f,
                aspectRatio,
                aperture,
                focusDistance);

            DateTime start = DateTime.Now;
            Console.WriteLine("Start at: {0}", start.ToString("F"));

            Parallel.For(0, imageWidth * imageHeight,
                index => {
                    int i = index % imageWidth;
                    int j = imageHeight - (index / imageWidth + 1);

                    Vector3 color = Vector3.Zero;
                    for (int s = 0; s < samplesPerPixel; ++s)
                    {
                        float u = (i + Utility.RandomF()) / (imageWidth - 1);
                        float v = (j + Utility.RandomF()) / (imageHeight - 1);
                        Ray ray = camera.getRay(u, v);
                        color += rayColor(ray, world, maxDepth);
                    }

                    // devide the color total by the number of samples and gamma-correct for gamma = 2.0
                    float scale = 1.0f / samplesPerPixel;
                    float r = color.X;
                    float g = color.Y;
                    float b = color.Z;
                    r = (float)Math.Sqrt(scale * r);
                    g = (float)Math.Sqrt(scale * g);
                    b = (float)Math.Sqrt(scale * b);
                    imageBuffer[3 * index + 0] = (byte)(255.999f * Utility.Clamp(b, 0.0f, 1.0f));
                    imageBuffer[3 * index + 1] = (byte)(255.999f * Utility.Clamp(g, 0.0f, 1.0f));
                    imageBuffer[3 * index + 2] = (byte)(255.999f * Utility.Clamp(r, 0.0f, 1.0f));
                }
            );

            Bitmap bitmap = new Bitmap(imageWidth, imageHeight, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(imageBuffer, 0));
            bitmap.Save("../image.png", ImageFormat.Png);
            bitmap.Dispose();

            DateTime end = DateTime.Now;
            Console.WriteLine("End at: {0}", end.ToString("F"));
            TimeSpan deltaTime = end - start;
            Console.WriteLine("Took {0} ms", deltaTime.Milliseconds);
        }
    }
}
