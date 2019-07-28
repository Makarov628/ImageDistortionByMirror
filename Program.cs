using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;



namespace ImageDistortionByMirror
{

    class Program
    {

        static void Main(string[] args)
        {
            var d = new Distortion(10000, "climb.png");
            d.StartImageDistortion();
            

            Console.ReadLine();
        }


    }
}