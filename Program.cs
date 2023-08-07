using System.Drawing;

namespace LumiConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //load image as bitmap
            Bitmap bitmap = new Bitmap("C:\\Users\\Gabriele\\Desktop\\tony.jpg");

            //calculate luminosity
            //var luminosity = new ImageUtils().CalculateLuminosity(bitmap);

            //Console.WriteLine("triplosette ent: " + luminosity );

            var newbitmap = new ImageUtils().AdjustLuminosity(bitmap, 0.20);

            newbitmap.Save("C:\\Users\\Gabriele\\Desktop\\tony3.jpg");

        }
    }
}