using System;
using System.IO;

namespace CsRay
{
    public static class Util
    {
        static readonly Random Random = new Random();

        /// <returns>原点を中心とする半径1のXY平面上の円の中のランダムな位置</returns>
        public static Vec3 RandomInUnitDisk()
        {
            while (true)
            {
                var p = 2.0 * (new Vec3(Rand(), Rand(), 0.0)) - (new Vec3(1.0, 1.0, 0.0));
                if (p.SquaredLength < 1.0)
                    return p;
            }
        }

        /// <returns>原点を中心とする半径1の球内のランダムな座標</returns>
        public static Vec3 RandomInUnitSphere()
        {
            while (true)
            {
                var p = 2.0 * new Vec3(Rand(), Rand(), Rand()) - new Vec3(1.0, 1.0, 1.0);
                if (p.SquaredLength < 1.0)
                    return p;
            }
        }

        public static double Rand() => Random.NextDouble();

        public static double Rand(double min, double max) => min + Random.NextDouble() * (max - min);

        public static int RandInt() => Random.Next();

        /// <summary>
        /// PNM形式のファイルに保存する
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="pixels">画素</param>
        /// <param name="filePath">ファイルパス</param>

        public static void SaveAsPpm(int width, int height, Rgb[] pixels, string filePath)
        {
            var ps = new StreamWriter(filePath);
            ps.WriteLine("P3");
            ps.WriteLine($"{width} {height}");
            ps.WriteLine("255");
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var p = pixels[x + y * width];
                    var p2 = new Vec3(Math.Sqrt(p.R), Math.Sqrt(p.G), Math.Sqrt(p.B));
                    var r = Math.Min((int)(255 * p2.X + 0.5), 255);
                    var g = Math.Min((int)(255 * p2.Y + 0.5), 255);
                    var b = Math.Min((int)(255 * p2.Z + 0.5), 255);
                    ps.WriteLine($"{r} {g} {b}");
                }
            }
            ps.Close();
        }

        public static Rgb RandomRgb(double min, double max)
        {
            var r = Rand(min, max);
            var g = Rand(min, max);
            var b = Rand(min, max);
            return new Rgb(r, g, b);
        }

        public static Rgb RandomRgb() => RandomRgb(0, 1);

        public static Rgb RandomSaturatedRgb(double s, double v)
        {
            var min = (1 - s) * v;
            var range = v - min;
            var h6 = Rand() * 6;
            if (h6 < 1)
            {
                var g = h6 * range + min;
                return new Rgb(v, g, min);
            }
            if (h6 < 2)
            {
                var r = (2 - h6) * range + min;
                return new Rgb(r, v, min);
            }
            if (h6 < 3)
            {
                var b = (h6 - 2) * range + min;
                return new Rgb(min, v, b);
            }
            if (h6 < 4)
            {
                var g = (4 - h6) * range + min;
                return new Rgb(min, g, v);
            }
            if (h6 < 5)
            {
                var r = (h6 - 4) * range + min;
                return new Rgb(r, min, v);
            }
            {
                var b = (6 - h6) * range + min;
                return new Rgb(v, min, b);
            }
        }
    }
}
