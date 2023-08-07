using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LumiConsole
{
    internal class ImageUtils
    {
        double Linearize(double color)
        {
            return color <= 0.04045 ? color / 12.92 : Math.Pow((color + 0.055) / 1.055, 2.4);
        }

        public double CalculateLuminosity(Bitmap bitmap)
        {
            double luminosity = 0;
            int width = bitmap.Width;
            int height = bitmap.Height;
            int pixelCount = width * height;

            var rect = new Rectangle(0, 0, width, height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var depth = Image.GetPixelFormatSize(data.PixelFormat) / 8; //bytes per pixel
            var buffer = new byte[width * height * depth];

            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            bitmap.UnlockBits(data);

            for (int i = 0; i < buffer.Length; i += depth)
            {
                double b = Linearize(buffer[i] / 255.0);
                double g = Linearize(buffer[i + 1] / 255.0);
                double r = Linearize(buffer[i + 2] / 255.0);

                luminosity += 0.2126 * r + 0.7152 * g + 0.0722 * b;
            }

            return luminosity / pixelCount;
        }

        double GammaCorrect(double color)
        {
            return color <= 0.0031308 ? 12.92 * color : 1.055 * Math.Pow(color, 1 / 2.4) - 0.055;
        }

        public Bitmap AdjustLuminosity(Bitmap bitmap, double targetLuminosity)
        {
            double currentLuminosity = CalculateLuminosity(bitmap);
            double adjustFactor = targetLuminosity / currentLuminosity;

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            int heightInPixels = data.Height;
            int widthInBytes = data.Width * bytesPerPixel;
            byte[] pixels = new byte[data.Height * data.Stride];

            Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);


            for (int y = 0; y < heightInPixels; y++)
            {
                int currentLine = y * data.Stride;
                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    int bIndex = currentLine + x;
                    int gIndex = bIndex + 1;
                    int rIndex = bIndex + 2;


                    double b = Linearize(pixels[bIndex] / 255.0);
                    double g = Linearize(pixels[gIndex] / 255.0);
                    double r = Linearize(pixels[rIndex] / 255.0);


                    b = b * adjustFactor;
                    g = g * adjustFactor;
                    r = r * adjustFactor;


                    b = Math.Min(1, GammaCorrect(b));
                    g = Math.Min(1, GammaCorrect(g));
                    r = Math.Min(1, GammaCorrect(r));



                    pixels[rIndex] = (byte)(r * 255);
                    pixels[gIndex] = (byte)(g * 255);
                    pixels[bIndex] = (byte)(b * 255);
                }
            }
            

            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            bitmap.UnlockBits(data);
            return bitmap;
        }
    }
}
