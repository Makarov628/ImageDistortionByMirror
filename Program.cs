using System;

namespace ImageDistortionByMirror
{

    class Program
    {

        static void Main(string[] args)
        {
            // var d = new Distortion(10000, "climb.png");
            // d.StartImageDistortion();
            
            // var dh1 = new DistortionHypothesis1(1000);
            
            
            // dh1.CalculateWidthHeight(2560, 1440, -155, -125, Axis.X, 1);

            // dh1.CalculateWidthHeight(2560, 1440, -155, -125, Axis.Y, 1);

            // dh1.CalculateWidthHeight(2560, 1440, -155, -125, Axis.Y, 3);

            // dh1.CalculateWidthHeight(2560, 1440, -155, -125, Axis.X, 2);
           
            var d = new Distortion(2000, "climb.png", 1000, 0);
            d.StartImageDistortion();

            // for (int i = 0; i <= 5000; i += 500)
            // {
            //     try
            //     {
            //         var d = new Distortion(5600, "wall-e.png", -1000, i);
            //         d.StartImageDistortion();
            //         Console.WriteLine($"climb_{2000}R created.");
            //     }
            //     catch (System.Exception e)
            //     {
            //         Console.ForegroundColor = ConsoleColor.Red;
            //         Console.WriteLine($"climb_{2000}R {e.Message}");
            //         Console.ForegroundColor = ConsoleColor.White;
            //     }
                
            // }

            
        }


    }
}