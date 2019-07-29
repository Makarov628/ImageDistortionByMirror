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
           
            var d = new Distortion(2600, "climb.png", 700, -100);
            d.StartImageDistortion();



            
        }


    }
}