using System;
using System.IO;

namespace ImageDistortionByMirror
{
    public class SizeData
    {
        public double maxX { get; set; }
        public double minX { get; set; }
        public double maxY { get; set; }
        public double minY { get; set; }

        public int width { get; set; }
        public int height { get; set; }
    }

    public enum Optimize
    {
        Maximum, Minimum
    }
    public enum Axis
    {
        X, Y
    }
    public class Distortion
    {
        public double r;
        public string image;

        public int offsetX;
        public int offsetY;
        public Distortion(double mirrorRadius, string imageFileName, int offsetX, int offsetY)
        {
            this.r = mirrorRadius;
            this.image = imageFileName;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        public void StartImageDistortion()
        {
            var bitmap = ImageManager.ImageToBitmap(image);
            var sizeData = CalculateSize(bitmap.Width, bitmap.Height);
            var newBitmap = ImageManager.CreateBitmap(sizeData.width, sizeData.height);

            for (int y = 1; y < bitmap.Height; y++)
            {
                for (int x = 1; x < bitmap.Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    var XY = DistortedCoordinate(x, y);

                    int newX = (int)(XY[0]) - (int)(sizeData.maxX);
                    int newY = (int)(XY[1]) - (int)(sizeData.minY);

                    if ((newBitmap.Width > newX && newBitmap.Height > newY) && (newX > 0 && newY > 0)) 
                    {
                         newBitmap.SetPixel(newX, newY, color);
                    }

                    
                }
            }

            ImageManager.SaveToImage(newBitmap, image, r);
        }

        /// <summary>Искажение точки зеркалом</summary>
        /// <param name="x1">Координата X</param>
        /// <param name="y1">Координата Y</param>
        /// <returns>Координаты (x, y)</returns>
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


        /// <summary>Вычисление точки искривления стороны картинки</summary>
        /// <param name="coordinate">Набор из трех чисел</param>
        /// <param name="optimize">Максимум или минимум</param>
        /// <param name="axis">Ось X или Y</param>
        /// <returns>Число</returns>
        public double OptimizeCoordinate(double[] coordinate, Optimize optimize, Axis axis)
        {

            double c1 = coordinate[0];
            double c2 = coordinate[1];
            double c3 = coordinate[2];
            double e = 0.00001;

            double f = (1 + Math.Sqrt(5)) / 2;

            while (Math.Abs(c2 - c1) > e)
            {

                double gold1 = c2 - (c2 - c1) / f;
                double gold2 = c1 + (c2 - c1) / f;

                var axisCondition = (axis == Axis.X);
                var axisIndex = axisCondition ? 0 : 1;

                double v1 = axisCondition ? DistortedCoordinate(c3, gold1)[axisIndex] : DistortedCoordinate(gold1, c3)[axisIndex];
                double v2 = axisCondition ? DistortedCoordinate(c3, gold2)[axisIndex] : DistortedCoordinate(gold2, c3)[axisIndex];

                // Maximum : v1 < v2 , Minimum : v1 > v2
                var optimizeCondition = (optimize == Optimize.Maximum) ? (v1 < v2) : (v1 > v2);

                if (optimizeCondition) { c1 = gold1; } else { c2 = gold2; }

            }

            double x = (c1 + c2) / 2;
            double y = axis == Axis.Y ? DistortedCoordinate(x, c3)[1] : DistortedCoordinate(c3, x)[0];

            return y;
        }

        /// <summary>Подсчет ширины и высоты конечной картинки</summary>
        /// <param name="w">Исходная ширина</param>
        /// <param name="h">Исходная высота</param>
        public SizeData CalculateSize(int w, int h)
        {
            // top left
            double x1 = 0 + offsetX;
            double y1 = 0 + offsetY;

            // top right
            double x2 = w + offsetX;
            double y2 = 0 + offsetY;

            // bottom left
            double x3 = 0 + offsetX;
            double y3 = h + offsetY;

            // bottom right
            double x4 = w + offsetX;
            double y4 = h + offsetY;

            var maxY = OptimizeCoordinate(new double[3] { x1, x2, y1 }, Optimize.Maximum, Axis.Y);
            var minY = OptimizeCoordinate(new double[3] { x1, x2, y3 }, Optimize.Minimum, Axis.Y);
            var maxX = OptimizeCoordinate(new double[3] { y1, y3, x2 }, Optimize.Maximum, Axis.X);
            var minX = OptimizeCoordinate(new double[3] { y1, y3, x1 }, Optimize.Minimum, Axis.X);


            Console.WriteLine($"minY: {minY}");
            Console.WriteLine($"maxY: {maxY}");
            Console.WriteLine($"minX: {minX}");
            Console.WriteLine($"maxX: {maxX}");

            return new SizeData() {
                maxX = maxX,
                minX = minX,
                maxY = maxY,
                minY = minY,
                width = (int)Math.Abs(maxX - minX) ,
                height = (int)Math.Abs(maxY - minY) 
            };
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