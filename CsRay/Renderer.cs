namespace CsRay
{
    public class Renderer
    {
        const double _tMin = 0.001;

        readonly Hittable _world;
        readonly Rgb? _background;

        public Renderer(Hittable world, Rgb? background)
        {
            _world = world;
            _background = background;
        }

        public Rgb[] Render(Camera camera, int width, int height, int maxDepth, int sampleCount)
        {
            var pixels = new Rgb[height * width];
#if false
            // 並列処理を行わない(39.2秒)
            for (var y = 0; y < height; y++)
            {
                Console.WriteLine($"{y}/{height}");
                for (var x = 0; x < width; x++)
                {
                    var rgbSum = new Rgb(0, 0, 0);
                    for (var i = 0; i < sampleCount; i++)
                    {
                        var u = (x + Random.Shared.NextDouble()) / width;
                        var v = ((height - 1) - y + Random.Shared.NextDouble()) / height;
                        var r = camera.GetRay(u, v);
                        var rgb = Color(r, maxDepth);
                        rgbSum += rgb;
                    }
                    pixels[y * width + x] = rgbSum / sampleCount;
                }
            }
#else
            var parallelOption = new ParallelOptions();
            //parallelOption.MaxDegreeOfParallelism = 4;
#if true
            var processedLineCount = 0;
            // Yループを並列処理(4->32.9秒)
            Parallel.For(0, height, parallelOption, y =>
            {
                for (var x = 0; x < width; x++)
                {
                    var rgbSum = new Rgb(0, 0, 0);
                    for (var i = 0; i < sampleCount; i++)
                    {
                        var u = (x + Random.Shared.NextDouble()) / width;
                        var v = ((height - 1) - y + Random.Shared.NextDouble()) / height;
                        var r = camera.GetRay(u, v);
                        var rgb = Color(r, maxDepth);
                        rgbSum += rgb;
                    }
                    pixels[y * width + x] = rgbSum / sampleCount;
                }
                processedLineCount++;
                Console.WriteLine($"{processedLineCount}/{height}");
            });
#else
            for (var y = 0; y < height; y++)
            {
                Console.WriteLine($"{y}/{height}");
#if false
                // Xループを並列処理(4->32.6秒)
                Parallel.For(0, width, parallelOption, x =>
                {
                    var rgbSum = new Rgb(0, 0, 0);
                    for (var i = 0; i < sampleCount; i++)
                    {
                        var u = (x + Util.Rand()) / width;
                        var v = ((height - 1) - y + Util.Rand()) / height;
                        var r = camera.GetRay(u, v);
                        var rgb = Color(r, maxDepth);
                        rgbSum += rgb;
                    }
                    pixels[y * width + x] = rgbSum / sampleCount;
                });
#else
                // サンプリングループを並列処理(4->86.4秒)
                for (var x = 0; x < width; x++)
                {
                    var rgbSum = new Rgb(0, 0, 0);
                    Parallel.For(0, sampleCount, i =>
                    {
                        var u = (x + Util.Rand()) / width;
                        var v = ((height - 1) - y + Util.Rand()) / height;
                        var r = camera.GetRay(u, v);
                        rgbSum += Color(r, maxDepth);
                    });
                    pixels[y * width + x] = rgbSum / sampleCount;
                }
#endif
            }
#endif
#endif
            return pixels;
        }

        /**
         * 色を返す
         * @param ray レイ
         * @param world 物体群
         * @param depth 残りの追跡回数
         * @return 色
         */
        Rgb Color(Ray ray, int depth)
        {
            // 追跡回数が規定値に達した場合は(0,0,0)を返す
            if (depth <= 0)
                return Rgb.Black;

            var rec = _world.Hit(ray, _tMin, double.MaxValue);
            if (rec == null)
            {
                // どの物体ともヒットしない場合は、指定された背景色あるいは天球の色を返す
                if (_background == null)
                {
                    var unitDirection = ray.Direction.Unit;
                    var t = 0.5 * (unitDirection.Y + 1);
                    var v1 = new Rgb(1, 1, 1);
                    var v2 = new Rgb(0.5, 0.7, 1);
                    return (1 - t) * v1 + t * v2;
                }
                else
                    return _background;
            }
#if true
            var emitted = rec.Material.Emitted(rec.U, rec.V, rec.Position);
            var s = rec.Material.Scatter(ray, rec);
            if (s == null)
                return emitted;
            return emitted + s.Value.Item1 * Color(s.Value.Item2, depth - 1);
#else
            return new Rgb(1, 0, 0);
#endif
        }
    }
}
