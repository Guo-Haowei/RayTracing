using System;

namespace RayTracingInOneWeekend
{
    class Program
    {
        static void Main()
        {
            const int image_width = 256;
            const int image_height = 256;

            Console.WriteLine("P3\n{0} {1}\n255", image_width, image_height);

            for (int j = image_height - 1; j >= 0; --j)
            {
                for (int i = 0; i < image_width; ++i)
                {
                    float r = (float)i / (image_width - 1);
                    float g = (float)j / (image_height - 1);
                    float b = 0.25f;
                    int ir = (int)(255.999f * r);
                    int ig = (int)(255.999f * g);
                    int ib = (int)(255.999f * b);

                    Console.WriteLine("{0} {1} {2}", ir, ig, ib);
                }
            }
        }
    }
}
