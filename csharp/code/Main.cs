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
        static readonly float lightXMin = 213.0f;
        static readonly float lightXMax = 343.0f;
        static readonly float lightZMin = 227.0f;
        static readonly float lightZMax = 332.0f;
        static readonly float boxSize = 555.0f;
        static readonly float lightY = boxSize - 1.0f;

        static Vector3 rayColor(in Ray ray, in Vector3 background, in Hittable light, in HittableList world, int depth)
        {
            if (depth-- <= 0)
                return Vector3.Zero;

            HitRecord hrec = new HitRecord();

            if (!(world.hit(ray, Ray.ZMin, Ray.ZMax, ref hrec)))
                return background;

            Material mat = hrec.material;
            Vector3 emitted = mat.emit(ray, hrec);
            ScatterRecord srec = new ScatterRecord();
            if (!mat.scatter(ray, hrec, ref srec))
                return emitted;

            if (srec.isSpecular)
            {
                return srec.attenuation * rayColor(srec.specularRay, background, light, world, depth);
            }

            var lightPdf = new HittablePdf(light, hrec.point);
            var pdf = new MixturePdf(lightPdf, srec.pdf);

            Ray scattered = new Ray(hrec.point, pdf.generate());

            float pdfValue = pdf.value(scattered.direction);
            float scatteredPdf = mat.scatterPdf(ray, hrec, scattered);

            return emitted +
                   srec.attenuation *
                   scatteredPdf *
                   rayColor(scattered, background, light, world, depth) / pdfValue;
        }

        static void createScene(out HittableList world, out Hittable light, out Camera camera, float aspect)
        {
            world = new HittableList();

            var red = new Lambertian(new SolidColor(0.65f, 0.05f, 0.05f));
            var green = new Lambertian(new SolidColor(0.12f, 0.45f, 0.15f));
            var white = new Lambertian(new SolidColor(0.73f));
            var emissive = new DiffuseLight(new SolidColor(10.0f));
            var aluminum = new Metal(new Vector3(0.8f, 0.85f, 0.88f), 0.0f);

            float s = boxSize;
            world.add(new YZRect(Vector3.Zero, s * Vector3.One, s, green));
            world.add(new YZRect(Vector3.Zero, s * Vector3.One, 0.0f, red));
            world.add(new XZRect(Vector3.Zero, s * Vector3.One, s, white));
            world.add(new XZRect(Vector3.Zero, s * Vector3.One, 0.0f, white));
            world.add(new XYRect(Vector3.Zero, s * Vector3.One, s, white));
            // add light
            {
                Vector3 lmin = new Vector3(lightXMin, 0.0f, lightZMin);
                Vector3 lmax = new Vector3(lightXMax, 0.0f, lightZMax);
                var rectLight = new XZRect(lmin, lmax, lightY, emissive);
                world.add(rectLight);
                light = rectLight;
            }
            // add sphere
            {
                var min = new Vector3(130.0f, 0.0f, 65.0f);
                var max = new Vector3(295.0f, 165.0f, 230.0f);
                var center = 0.5f * (min + max);
                var radius = 0.5f * (max - min).X;
                var sphere = new Sphere(center, radius, white);
                world.add(sphere);
            }
            // add box
            {
                var min = new Vector3(265.0f, 0.0f, 295.0f);
                var max = new Vector3(430.0f, 330.0f, 460.0f);
                world.add(new Box(min, max, aluminum));
            }

            Vector3 lookFrom = new Vector3(278.0f, 278.0f, -800.0f);
            Vector3 lookAt = new Vector3(278.0f, 278.0f, 0.0f);
            float focusDistance = 10.0f;
            float aperture = 0.0f;
            float fov = 40.0f;
            camera = new Camera(
                lookFrom,
                lookAt,
                Vector3.UnitY,
                fov,
                aspect,
                aperture,
                focusDistance,
                0.0f,
                1.0f);

        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: $./RayTracingInOneWeekend samples [width] [height]");
                return;
            }

            DateTime start = DateTime.Now;
            Console.WriteLine("Start at: {0}", start.ToString("F"));

            int samplesPerPixel = 10;
            int imageWidth = 512;
            int imageHeight = imageWidth;

            if (args.Length > 0)
                samplesPerPixel = int.Parse(args[0]);

            if (args.Length > 1)
                imageWidth = imageHeight = int.Parse(args[1]);

            if (args.Length > 2)
                imageHeight = int.Parse(args[2]);

            Console.WriteLine("Samples per pixel: {0}", samplesPerPixel);
            Console.WriteLine("Image extent: {0} x {1}", imageWidth, imageHeight);

            int maxDepth = 50;
            int component = 3;
            int stride = component * imageWidth;

            float aspectRatio = (float)imageWidth / imageHeight;

            byte[] imageBuffer = new byte[stride * imageHeight];

            Camera camera;
            Hittable light;
            HittableList world;

            createScene(out world, out light, out camera, aspectRatio);


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
                        Vector3 c = rayColor(ray, Vector3.Zero, light, world, maxDepth);
                        bool isColorNaN = float.IsNaN(c.X) || float.IsNaN(c.Y) || float.IsNaN(c.Z);
                        c = isColorNaN ? Vector3.Zero : c;
                        color += c;
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
