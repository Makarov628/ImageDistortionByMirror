using System.IO;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ImageDistortionByMirror
{
    public class ImageManager
    {
        
        public static Bitmap ImageToBitmap(string name)
        {
            var image = Image.FromFile(name);
            return new Bitmap(image);
        }


        public static Bitmap CreateBitmap(int w, int h)
        {
            return new Bitmap(w, h);
        }

        public static void SaveToImage(Bitmap bitmap, string sourceImage, double r)
        {
            var directory = Environment.CurrentDirectory + "/output/" + sourceImage.Split(".")[0] + "_R" + r.ToString();
            var name = DateTime.Now.ToString("yyyyMMdd-HHmmss-ffffff") +  ".png";
            var path = string.Concat(directory, "/", name);

            Directory.CreateDirectory(directory);

            bitmap.Save(path);
            Console.WriteLine($"Saved in {directory}/{name}");
        }

    }
}