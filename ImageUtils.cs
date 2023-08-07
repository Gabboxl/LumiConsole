using System.Drawing;

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
            int pixelCount = 0;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    double b = Linearize(pixel.B / 255.0);
                    double g = Linearize(pixel.G / 255.0);
                    double r = Linearize(pixel.R / 255.0);
                    luminosity += 0.2126 * r + 0.7152 * g + 0.0722 * b;
                    pixelCount++;
                }
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
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    double b = Linearize(pixel.B / 255.0);
                    double g = Linearize(pixel.G / 255.0);
                    double r = Linearize(pixel.R / 255.0);
                    b = b * adjustFactor;
                    g = g * adjustFactor;
                    r = r * adjustFactor;
                    b = Math.Min(1, GammaCorrect(b));
                    g = Math.Min(1, GammaCorrect(g));
                    r = Math.Min(1, GammaCorrect(r));
                    bitmap.SetPixel(x, y, Color.FromArgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255)));
                }
            }
            return bitmap;
        }
    }
}
