using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RayTracingInOneWeekend
{
    class Program
    {
        static void Main()
        {
            const int imageWidth = 256;
            const int imageHeight = 256;
            const int component = 3;
            const int stride = component * imageWidth;

            byte[] imageBuffer = new byte[stride * imageHeight];

            // Console.WriteLine("P3\n{0} {1}\n255", imageWidth, imageHeight);

            int index = 0;
            for (int j = imageHeight - 1; j >= 0; --j)
            {
                for (int i = 0; i < imageWidth; ++i)
                {
                    float r = (float)i / (imageWidth - 1);
                    float g = (float)j / (imageHeight - 1);
                    float b = 0.25f;
                    byte br = (byte)(255.999f * r);
                    byte bg = (byte)(255.999f * g);
                    byte bb = (byte)(255.999f * b);

                    imageBuffer[index++] = bb;
                    imageBuffer[index++] = bg;
                    imageBuffer[index++] = br;
                }
            }

            Bitmap bitmap = new Bitmap(imageWidth, imageHeight, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(imageBuffer, 0));
            bitmap.Save("./my.png", ImageFormat.Png);
            bitmap.Dispose();
        }
    }
}
