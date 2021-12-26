using System;

namespace CsRay
{
    public static class Renderer
    {
        const int _maxDepth = 50;
        const double _tMin = 0.001;

        public static Rgb[] Render(Hittable world, Camera camera, int width, int height, int sampleCount, Rgb background = null)
        {
            var t0 = DateTime.Now;
            var pixels = new Rgb[height * width];
            for (var y = 0; y < height; y++)
            {
                Console.WriteLine($"{y}/{height}");
                for (var x = 0; x < width; x++)
                {
                    var rgbSum = new Rgb(0, 0, 0);
                    for (var i = 0; i < sampleCount; i++)
                    {
                        var u = (x + Util.Rand()) / width;
                        var v = ((height - 1) - y + Util.Rand()) / height;
                        var r = camera.GetRay(u, v);
                        var rgb = Color(r, background, world, _maxDepth);
                        rgbSum += rgb;
                    }
                    pixels[y * width + x] = rgbSum / sampleCount;
                }
            }
            var t = (DateTime.Now - t0).TotalMilliseconds;
            Console.WriteLine($"time = {t / 1000}.{t % 1000}");
            return pixels;
        }

        /**
         * 色を返す
         * @param ray レイ
         * @param world 物体群
         * @param depth レイの残りの反射回数
         * @return 色
         */
        static Rgb Color(Ray ray, Rgb background, Hittable world, int depth)
        {
            // 反射回数が規定値に達した場合は(0,0,0)を返す
            if (depth <= 0)
                return new Rgb(0, 0, 0);

            var rec = new HitRecord(0, new Vec3(0, 0, 0), new Vec3(0, 0, 0), null);
            if (!world.Hit(ray, _tMin, double.MaxValue, ref rec))
            {
                // どの物体ともヒットしない場合は、指定された背景色あるいは天球の色を返す
                if (background == null)
                {
                    var unitDirection = ray.Direction.Unit;
                    var t = 0.5 * (unitDirection.Y + 1.0);
                    var v1 = new Rgb(1.0, 1.0, 1.0);
                    var v2 = new Rgb(0.5, 0.7, 1.0);
                    return (1.0 - t) * v1 + t * v2;
                }
                else
                {
                    return background;
                }
            }
#if true
            var emitted = rec.Material.Emitted(rec.U, rec.V, rec.Position);
            if (!rec.Material.Scatter(ray, ref rec, out var attenuation, out var scattered))
                return emitted;
            return emitted + attenuation * Color(scattered, background, world, depth - 1);
#else
            return new Rgb(1, 0, 0);
#endif
        }
    }
}
