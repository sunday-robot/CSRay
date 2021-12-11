using System;

namespace CsRay
{
    public static class Renderer
    {
        const int _maxDepth = 50;
        const double _tMin = 0.001;
        static readonly Rgb _backGround = new Rgb(0.5, 0.5, 0.5);

        public static Rgb[] Render(Hittable world, Camera camera, int width, int height, int sampleCount)
        {
            var t0 = DateTime.Now;
            var pixels = new Rgb[height * width];
            for (var y = 0; y < height; y++)
            {
                Console.WriteLine($"{y}/{height}");
                for (var x = 0; x < width; x++)
                {
                    var col = new Rgb(0.0, 0.0, 0.0);
                    for (var i = 0; i < sampleCount; i++)
                    {
                        var u = (x + Util.Rand()) / width;
                        var v = ((height - 1) - y + Util.Rand()) / height;
                        var r = camera.GetRay(u, v);
                        col += RayColor(r, _backGround, world, _maxDepth);
                    }
                    pixels[y * width + x] = col / sampleCount;
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
        static Rgb RayColor(Ray ray, Rgb backGround, Hittable world, int depth)
        {
            // 反射回数が規定値に達した場合は(0,0,0)を返す
            if (depth <= 0)
                return new Rgb(0, 0, 0);

            var rec = new HitRecord(0, new Vec3(0, 0, 0), new Vec3(0, 0, 0), null);
            if (!world.Hit(ray, _tMin, double.MaxValue, ref rec))
            {
#if true
                // どの物体ともヒットしない場合は、天球の色を返す
                var unitDirection = ray.Direction.Unit;
                var t = 0.5 * (unitDirection.Y + 1.0);
                var v1 = new Rgb(1.0, 1.0, 1.0);
                var v2 = new Rgb(0.5, 0.7, 1.0);
                return (1.0 - t) * v1 + t * v2;
#else
                return backGround;
#endif
            }
#if true
            var emitted = rec.Material.Emitted(rec.U, rec.V, rec.Position);
            if (!rec.Material.Scatter(ray, ref rec, out var attenuation, out var scattered))
                return emitted;
            return emitted + attenuation * RayColor(scattered, backGround, world, depth - 1);
#else
            return new Rgb(1, 0, 0);
#endif
        }
    }
}
