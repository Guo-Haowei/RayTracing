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
        static Vector3 rayColor(in Ray ray, in Vector3 background, in HittableList world, int depth)
        {
            if (depth <= 0)
                return Vector3.Zero;

            HitRecord hitRecord = new HitRecord();

            if (!(world.hit(ray, 0.001f, float.PositiveInfinity, ref hitRecord)))
                return background;

            Ray scattered = new Ray();
            scattered.time = 0.0f;
            Vector3 attenuation = new Vector3();
            Material mat = hitRecord.material;
            Vector3 emitted = mat.emit(hitRecord.uv, hitRecord.point);
            if (!mat.scatter(ray, hitRecord, ref attenuation, ref scattered))
                return emitted;
            
            return emitted + attenuation * rayColor(scattered, background, world, depth - 1);
        }

        static HittableList createScene()
        {
            HittableList world = new HittableList();

            var red = new Lambertian(new SolidColor(0.65f, 0.05f, 0.05f));
            var green = new Lambertian(new SolidColor(0.12f, 0.45f, 0.15f));
            var white = new Lambertian(new SolidColor(0.73f));
            var light = new DiffuseLight(new SolidColor(15.0f));

            float s = 555.0f;
            world.add(new YZRect(Vector3.Zero, s * Vector3.One, s, green));
            world.add(new YZRect(Vector3.Zero, s * Vector3.One, 0.0f, red));
            world.add(new XZRect(Vector3.Zero, s * Vector3.One, s, white));
            world.add(new XZRect(Vector3.Zero, s * Vector3.One, 0.0f, white));
            world.add(new XYRect(Vector3.Zero, s * Vector3.One, s, white));
            world.add(new XZRect(new Vector3(213.0f), new Vector3(343.0f), s - 1, light));

            world.add(new Box(new Vector3(130.0f, 0.0f, 65.0f), new Vector3(295.0f, 165.0f, 230.0f), white));
            world.add(new Box(new Vector3(265.0f, 0.0f, 295.0f), new Vector3(430.0f, 330.0f, 460.0f), white));

            return world;
        }

        static void Main()
        {
            const float aspectRatio = 16.0f / 9.0f;
            const int imageWidth = 384;
            const int imageHeight = (int)(imageWidth / aspectRatio);
            const int samplesPerPixel = 500;
            const int maxDepth = 30;

            const int component = 3;
            const int stride = component * imageWidth;

            byte[] imageBuffer = new byte[stride * imageHeight];

            HittableList world = createScene();

            Vector3 lookFrom = new Vector3(278.0f, 278.0f, -800.0f);
            Vector3 lookAt = new Vector3(278.0f, 278.0f, 0.0f);
            float focusDistance = 10.0f;
            float aperture = 0.0f;
            float fov = 40.0f;
            Camera camera = new Camera(
                lookFrom,
                lookAt,
                Vector3.UnitY,
                fov,
                aspectRatio,
                aperture,
                focusDistance,
                0.0f,
                1.0f);

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
                        color += rayColor(ray, Vector3.Zero, world, maxDepth);
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
            Console.WriteLine("Took: {0} hours {1} minutes {2} seconds", deltaTime.Hours, deltaTime.Minutes, deltaTime.Seconds + Math.Round(deltaTime.Milliseconds / 1000.0, 3));
        }
    }
}
