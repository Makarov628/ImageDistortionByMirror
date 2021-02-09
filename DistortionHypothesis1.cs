using System;
using System.IO;
 
namespace ImageDistortionByMirror
{
    public class DistortionHypothesis1
    {
        public double r;
        public DistortionHypothesis1(double radius) 
        {
            r = radius;
        }   

        public void CalculateWidthHeight(int w, int h, int shiftX, int shiftY, Axis axisConst, int direction)
        {
            // top left
            int x1 = 0 + shiftX;
            int y1 = 0 + shiftY;

            // top right
            int x2 = w + shiftX;
            int y2 = 0 + shiftY;

            // bottom left
            int x3 = 0 + shiftX;
            int y3 = h + shiftY;

            // bottom right
            int x4 = w + shiftX;
            int y4 = h + shiftY;

            File.AppendAllText("dots1.csv", $"{x1};{y1}\n");
            File.AppendAllText("dots1.csv", $"{x2};{y2}\n");
            File.AppendAllText("dots1.csv", $"{x3};{y3}\n");
            File.AppendAllText("dots1.csv", $"{x4};{y4}\n");

            var length =  0;//(axisConst == Axis.X) ? h : w;

            if (axisConst == Axis.X) { length = h; }
            if (axisConst == Axis.Y) { length = w; }

            var x = 0;
            var y = 0;

            switch (direction)
            {
                case 1: 
                    x = x1;
                    y = y1;
                break;
                
                case 2: 
                    x = x2;
                    y = y2;
                break;

                case 3: 
                    x = x3;
                    y = y3;
                break;

                case 4: 
                    x = x4;
                    y = y4;
                break;

                default: break;
            }
            
            

            for (int i = 0; i < length; i++)
            {
                if (axisConst == Axis.X)
                {
                    var dc = DistortedCoordinate(x, y + i);
                    Console.WriteLine($"{dc[0]};{dc[1]}");//Console.WriteLine($"({x1}, {y1 + i}) | ({dc[0]}, {dc[1]})");
                    File.AppendAllText("dots1.csv", $"{dc[0]};{dc[1]}\n");
                } 
                
                if (axisConst == Axis.Y) {
                    var dc = DistortedCoordinate(x + i, y);
                    Console.WriteLine($"{dc[0]};{dc[1]}");//Console.WriteLine($"({x1 + i}, {y1}) | ({dc[0]}, {dc[1]})");
                    File.AppendAllText("dots1.csv", $"{dc[0]};{dc[1]}\n");
                }
            }
            
            
        }

        public double[] DistortedCoordinate(double x1, double y1)
        {
            var x2 = x1 == 0 ? 0.001 : x1;
            var y2 = y1 == 0 ? 0.001 : y1;

            var r2 = r / 2;

            var k1 = (y2 / x2);
            var k2 = y2 / (r2 - sqrt(sqr(r) - sqr(y2)));
            var b = k2 * r2;

            var x = b / (k1 - k2);
            var y = x * k1;

            return new double[2] { x, y };
        }

        private double sqrt(double x)
        {
            return Math.Sqrt(x);
        }

        private double sqr(double x)
        {
            return Math.Pow(x, 2);
        }
    }
}
