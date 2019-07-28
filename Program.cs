using System;

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