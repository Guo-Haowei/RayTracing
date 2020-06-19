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

        static HittableList randomScene()
        {
            HittableList world = new HittableList();

            var groundMaterial = new Lambertian(new CheckerTexture(new SolidColor(0.2f, 0.3f, 0.1f), new SolidColor(0.9f)));
            // var groundMaterial = new Lambertian(new SolidColor(0.5f, 0.5f, 0.5f));
            world.add(new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, groundMaterial));

            for (int a = -11; a < 11; ++a)
            {
                for (int b = -11; b < 11; ++b)
                {
                    var whichMat = Utility.RandomF();
                    Vector3 center = new Vector3(a + 0.9f * Utility.RandomF(), 0.2f, b + 0.9f * Utility.RandomF());

                    Material mat = null;

                    if (Vector3.Distance(center, new Vector3(4.0f, 0.2f, 0.0f)) <= 0.9f)
                        continue;

                    if (whichMat < 0.8f)
                    {
                        mat = new Lambertian(new SolidColor(Utility.RandomColor() * Utility.RandomColor()));
                        Vector3 center1 = center + new Vector3(0.0f, Utility.RandomF(0.0f, 0.5f), 0.0f);
                        world.add(new MovingSphere(center, center1, 0.0f, 1.0f, 0.2f, mat));
                        continue;
                    }
                    else if (whichMat < 0.95f)
                        mat = new Metal(Utility.RandomColor(0.5f, 1.0f), Utility.RandomF(0.0f, 0.5f));
                    else
                        mat = new Dielectric(1.5f);
                    
                    world.add(new Sphere(center, 0.2f, mat));
                }
            }

            // var material1 = new Dielectric(1.5f);
            var material1 = new DiffuseLight(new SolidColor(1.0f));
            world.add(new Sphere(new Vector3(0.0f, 1.0f, 0.0f), 1.0f, material1));

            var material2 = new Lambertian(new SolidColor(0.4f, 0.2f, 0.1f));
            world.add(new Sphere(new Vector3(-4.0f, 1.0f, 0.0f), 1.0f, material2));

            var material3 = new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f);
            world.add(new Sphere(new Vector3(4.0f, 1.0f, 0.0f), 1.0f, material3));

            return world;
        }

        static void Main()
        {
            const float aspectRatio = 16.0f / 9.0f;
            const int imageWidth = 384;
            const int imageHeight = (int)(imageWidth / aspectRatio);
            const int samplesPerPixel = 100;
            const int maxDepth = 20;
            // const int maxDepth = 50;

            const int component = 3;
            const int stride = component * imageWidth;

            byte[] imageBuffer = new byte[stride * imageHeight];

            HittableList world = randomScene();
            // HittableList world = new HittableList();
            // world.add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, new Lambertian(new Vector3(0.1f, 0.2f, 0.5f))));
            // world.add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, new Lambertian(new Vector3(0.8f, 0.8f, 0.0f))));
            // world.add(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, new Metal(new Vector3(0.8f, 0.6f, 0.2f), 0.3f)));
            // world.add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, new Dielectric(1.5f)));
            // world.add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), -0.45f, new Dielectric(1.5f)));

            // Vector3 lookFrom = new Vector3(3.0f, 3.0f, 2.0f);
            // Vector3 lookAt = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 lookFrom = new Vector3(13.0f, 2.0f, 3.0f);
            Vector3 lookAt = Vector3.Zero;
            float focusDistance = 10.0f;
            float aperture = 0.1f;
            Camera camera = new Camera(
                lookFrom,
                lookAt,
                Vector3.UnitY,
                20.0f,
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
