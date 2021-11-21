using CsRay.Hitables;
using CsRay.Materials;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static CsRay.Util;

namespace CsRay
{
    class Program
    {
        static void Main()
        {
            var imageWidth = 1280;
            var imageHeight = 720;
            var overSamplingCount = 10;

            var world = new HitableList(CreateScene());
            var cam = CreateCamera(((double)imageWidth) / imageHeight);

            var pixels = Renderer.Render(world, cam, imageWidth, imageHeight, overSamplingCount);

            try
            {
                SaveAsPpm(imageWidth, imageHeight, pixels, "Spheres.ppm");
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Debug.WriteLine(e);
            }
        }

        static Camera CreateCamera(double aspect)
        {
#if true
            var lookFrom = new Vec3(13.0, 2.0, 3.0);
            var lookAt = new Vec3(0.0, 0.0, 0.0);
            var vFov = 20.0;
            var aperture = 0.1;
            var distanceToFocus = 10.0;
#else
            var lookFrom = new Vec3(0, 0, 3.0);
            var lookAt = new Vec3(0, 0, 0);
            var vFov = 20;
            var aperture = 0.1;
            var distanceToFocus = 10.0;
#endif
            var cam = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0.0, 1.0, 0.0), vFov, aspect, aperture, distanceToFocus, 0.0, 0.0);
            return cam;
        }

        /// <summary>
        /// 地面となる非常に大きな球と、三つの大きな球と、多数の小さな球を生成する
        /// </summary>
        /// <returns></returns>
        static List<Hittable> CreateScene()
        {
            var hitables = new List<Hittable>();
#if true
            // 多数の小さな級
            for (var a = -11; a < 10; a++)
            {
                for (var b = -11; b < 10; b++)
                {
                    var center = new Vec3(a + 0.9 * Rand(), 0.2, b + 0.9 * Rand());
                    if ((center - (new Vec3(4.0, 0.2, 0.0))).Length <= 0.9)
                        continue;
                    Material material;
                    var chooseMat = Rand();
                    if (chooseMat < 0.8)
                    {
                        // ざらついたプラスチックのような素材
                        material = new Lambertian(new Rgb(Rand() * Rand(), Rand() * Rand(), Rand() * Rand()));
                    }
                    else if (chooseMat < 0.95)
                    {
                        // 金属
                        material = new Metal(new Rgb(0.5 * (1 + Rand()), 0.5 * (1 + Rand()), 0.5 * (1 + Rand())),
                                0.5 * Rand());
                    }
                    else
                    {
                        // ガラス
                        material = new Dielectric(1.5);
                    }
                    hitables.Add(new Sphere(center, 0.2, material));
                }
            }

            // 三つの大きな球
            hitables.Add(new Sphere(new Vec3(0.0, 1.0, 0.0), 1.0, new Dielectric(1.5)));
            hitables.Add(new Sphere(new Vec3(-4.0, 1.0, 0.0), 1.0, new Lambertian(new Rgb(0.4, 0.2, 0.1))));
            hitables.Add(new Sphere(new Vec3(4.0, 1.0, 0.0), 1.0, new Metal(new Rgb(0.7, 0.6, 0.5), 0.0)));

            // 地面となる非常に大きな球
            hitables.Add(new Sphere(new Vec3(0.0, -1000.0, 0.0), 1000.0, new Lambertian(new Rgb(0.5, 0.5, 0.5))));
#else
            hitables.Add(new Sphere(new Vec3(0, 0, 0), 0.5, new Lambertian(new Rgb(1, 0.5, 0.5))));
#endif
            return hitables;
        }
    }
}
