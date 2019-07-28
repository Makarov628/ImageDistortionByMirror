using System;

namespace ImageDistortionByMirror
{
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
        public Distortion(double mirrorRadius, string imageFileName)
        {
            r = mirrorRadius;
            image = imageFileName;
        }

        public void StartImageDistortion()
        {
            var bitmap = ImageManager.ImageToBitmap(image);
            var lastXY = DistortedCoordinate(bitmap.Width, bitmap.Height);
            var newBitmap = ImageManager.CreateBitmap((int)lastXY[0], (int)lastXY[1]);

            for (int y = 1; y < bitmap.Height; y++)
            {
                for (int x = 1; x < bitmap.Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);

                    var newXY = DistortedCoordinate(x, y);

                    newBitmap.SetPixel((int)newXY[0], (int)newXY[1], color);

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
            var x2 = x1 * -1;
            var y2 = y1 * -1;

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
            double e = 0.001;

            double f = (1 + Math.Sqrt(5)) / 2;

            while (Math.Abs(c2 - c1) > e)
            {

                double gold1 = c2 - (c2 - c1) / f;
                double gold2 = c1 + (c2 - c1) / f;

                var axisIndex = (axis == Axis.X) ? 0 : 1;

                double v1 = DistortedCoordinate(c3, gold1)[axisIndex];
                double v2 = DistortedCoordinate(c3, gold2)[axisIndex];

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
        /// <param name="shiftX">Сдвиг по X</param>
        /// <param name="shiftY">Сдвиг по Y</param>
        public void CalculateWidthHeight(int w, int h, int shiftX, int shiftY)
        {
            // top left
            double x1 = 0 + shiftX;
            double y1 = 0 + shiftY;

            // top right
            double x2 = w + shiftX;
            double y2 = 0 + shiftY;

            // bottom left
            double x3 = 0 + shiftX;
            double y3 = h + shiftY;

            // bottom right
            double x4 = w + shiftX;
            double y4 = h + shiftY;



            // Distort 4 points
            var xy1 = DistortedCoordinate(x1, y1);
            var xy2 = DistortedCoordinate(x2, y2);
            var xy3 = DistortedCoordinate(x3, y3);
            var xy4 = DistortedCoordinate(x4, y4);

            // top left
            var xx1 = xy1[0];
            var yy1 = xy1[1];

            // top right
            var xx2 = xy2[0];
            var yy2 = xy2[1];

            // bottom left
            var xx3 = xy3[0];
            var yy3 = xy3[1];

            // bottom right
            var xx4 = xy4[0];
            var yy4 = xy4[1];


            // x1 x2 y1 -> min y
            // x1 x2 y2 -> max y
            // y1 y2 x1 -> min x
            // y1 y2 x2 -> max x

            // Почему мы передаем три координаты?
            // Если нам нужно узнать как изогнулась линия на вход нужно подавать (x1 y1) и (x2 y2)

            var minY = OptimizeCoordinate(new double[3] { xx1, xx2, yy1 }, Optimize.Minimum, Axis.Y);
            var maxY = OptimizeCoordinate(new double[3] { xx1, xx2, yy2 }, Optimize.Maximum, Axis.Y);
            var minX = OptimizeCoordinate(new double[3] { yy1, yy2, xx1 }, Optimize.Minimum, Axis.X);
            var maxX = OptimizeCoordinate(new double[3] { yy1, yy2, xx2 }, Optimize.Maximum, Axis.X);


            Console.WriteLine($"minY: {minY}");
            Console.WriteLine($"maxY: {maxY}");
            Console.WriteLine($"minX: {minX}");
            Console.WriteLine($"maxX: {maxX}");

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