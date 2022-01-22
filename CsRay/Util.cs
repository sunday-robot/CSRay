namespace CsRay
{
    public static class Util
    {
        static readonly Random Random = new();
        //static readonly MyRand Random = new();    // 乱数生成処理、乱数生成オブジェクトのロック処理を簡素化すると高速化されるかと期待したが、ほとんど効果はなかった。

        /// <returns>原点を中心とする半径1のXY平面上の円の中のランダムな位置</returns>
        public static Vec3 RandomInUnitDisk()
        {
            while (true)
            {
                var p = new Vec3(2 * Rand() - 1, 2 * Rand() - 1, 0);
                if (p.SquaredLength < 1.0)
                    return p;
            }
        }

        /// <returns>原点を中心とする半径1の球内のランダムな座標</returns>
        public static Vec3 RandomInUnitSphere()
        {
            while (true)
            {
                var p = 2 * new Vec3(Rand(), Rand(), Rand()) - new Vec3(1, 1, 1);
                if (p.SquaredLength < 1)
                    return p;
            }
        }

        public static float Rand()
        {
#if true
            float r;
            lock (Random)
            {
                r = Random.NextSingle();
            }
            return r;
#else
            return Random.NextSingle();
#endif
        }
        public static float Rand(float min, float max) => min + Rand() * (max - min);

        public static int RandInt() => Random.Next();

        /// <summary>
        /// BMP形式のファイルに保存する
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="pixels">画素</param>
        /// <param name="filePath">ファイルパス</param>

        public static void SaveAsBmp(int width, int height, Rgb[] pixels, string filePath)
        {
            var data = new byte[width * height * 3];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var p = pixels[x + y * width];
                    var p2 = new Vec3(MathF.Sqrt(p.R), MathF.Sqrt(p.G), MathF.Sqrt(p.B));
                    var r = Math.Min((int)(255 * p2.X + 0.5F), 255);
                    var g = Math.Min((int)(255 * p2.Y + 0.5F), 255);
                    var b = Math.Min((int)(255 * p2.Z + 0.5F), 255);
                    data[(y * width + x) * 3] = (byte)b;
                    data[(y * width + x) * 3 + 1] = (byte)g;
                    data[(y * width + x) * 3 + 2] = (byte)r;
                }
            }
            Bmp.Save(filePath, data, width, height);
        }

        public static Rgb RandomRgb(float min, float max)
        {
            var r = Rand(min, max);
            var g = Rand(min, max);
            var b = Rand(min, max);
            return new Rgb(r, g, b);
        }

        public static Rgb RandomRgb() => RandomRgb(0, 1);

        public static Rgb RandomSaturatedRgb(float s, float v)
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
