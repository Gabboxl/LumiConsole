using System.Drawing;

namespace LumiConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to LumiConsole!");


            //define a folder and interate for every image
            string[] files = Directory.GetFiles("C:\\Users\\Gabriele\\source\\repos\\FluidWeather\\FluentWeather\\Assets\\bgs", "*.jpg");
            foreach (string file in files)
            {
                //adjust luminosity for every image and save it with the same name but in a different folder
                Bitmap bitmap2 = new Bitmap(file);

                //calculate luminosity and print it in console with the name of the image
                var luminosity = new ImageUtils().CalculateLuminosity(bitmap2);
                //Console.WriteLine(Path.GetFileName(file) + " " + luminosity);

                if (luminosity > 0.15)
                {
                    //take input luminosity from the user
                    Console.WriteLine("Insert the luminosity you want to set for the image " + Path.GetFileName(file) + " (" + luminosity + ")");
                    double targetLuminosity = Convert.ToDouble(Console.ReadLine());


                    
                    var newbitmap = new ImageUtils().AdjustLuminosity(bitmap2, targetLuminosity);
                    newbitmap.Save("C:\\Users\\Gabriele\\Desktop\\newimgfixed\\" + Path.GetFileName(file));
                }

            }
        }
    }
}