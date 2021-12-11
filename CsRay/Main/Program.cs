using CsRay.Hittables;
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
#if true
            var imageWidth = 1280;
            var imageHeight = 720;
            var overSamplingCount = 10;
#else
            var imageWidth = 320;
            var imageHeight = 180;
            var overSamplingCount = 10;
#endif

            //var hittables = CreateSimpleScene();
            //var hittables = CreateSimpleScene2();
            //var hittables = CreateScene0();
            //var hittables = CreateScene1();
            var hittables = CreateScene2();
            //var hittables = CreateRandomScene();
#if true
            var world = new BvhNode(hittables, 0, 1);
            world.Print();
#else
            var world = new HittableList(hittables);
#endif

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
            var lookFrom = new Vec3(0, 1, 10);
            var lookAt = new Vec3(0, 1, 0);
            var vFov = 20;
            var aperture = 0.1;
            var distanceToFocus = 10.0;
#endif
            var time0 = 0.0;
            var time1 = 1.0;
            var cam = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0.0, 1.0, 0.0), vFov, aspect, aperture, distanceToFocus, time0, time1);
            return cam;
        }

        /// <summary>
        /// 世界の中心に、半径1のグレイの球のみがあるシーンを生成する。
        /// </summary>
        static List<Hittable> CreateSimpleScene()
        {
            var hittables = new List<Hittable>();
            hittables.Add(new Sphere(new Vec3(0.0, 0, 0.0), 1.0, new Lambertian(new Rgb(0.5, 0.5, 0.5))));
            return hittables;
        }

        /// <summary>
        /// 世界の中心に、半径1のグレイの球のみがあるシーンを生成する。
        /// </summary>
        static List<Hittable> CreateSimpleScene2()
        {
            var hittables = new List<Hittable>();
            hittables.Add(new Sphere(new Vec3(-2, 0, 0.0), 1.0, new Lambertian(new Rgb(1, 0, 0))));
            hittables.Add(new Sphere(new Vec3(0, 0, 0.0), 1.0, new Lambertian(new Rgb(0, 1, 0))));
            hittables.Add(new Sphere(new Vec3(2, 0, 0.0), 1.0, new Lambertian(new Rgb(0, 0, 1))));
            return hittables;
        }

        /// <summary>
        /// 地面となる非常に大きな球と、三つの大きな球と、多数の小さな球を生成する
        /// </summary>
        /// <returns></returns>
        static List<Hittable> CreateScene0()
        {
            var hittables = new List<Hittable>();
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
                    hittables.Add(new Sphere(center, 0.2, material));
                }
            }

            // 三つの大きな球
            hittables.Add(new Sphere(new Vec3(0.0, 1.0, 0.0), 1.0, new Dielectric(1.5)));
            hittables.Add(new Sphere(new Vec3(-4.0, 1.0, 0.0), 1.0, new Lambertian(new Rgb(0.4, 0.2, 0.1))));
            hittables.Add(new Sphere(new Vec3(4.0, 1.0, 0.0), 1.0, new Metal(new Rgb(0.7, 0.6, 0.5), 0.0)));

            // 地面となる非常に大きな球
            hittables.Add(new Sphere(new Vec3(0.0, -1000.0, 0.0), 1000.0, new Lambertian(new Rgb(0.5, 0.5, 0.5))));
            return hittables;
        }

        static List<Hittable> CreateRandomScene()
        {
            var world = new List<Hittable>();

            var checker = new CheckerTexture(new Rgb(0.2, 0.3, 0.1), new Rgb(0.9, 0.9, 0.9));

            world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(checker)));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    var chooseMat = Util.Rand();
                    var center = new Vec3(a + 0.9 * Util.Rand(), 0.2, b + 0.9 * Util.Rand());

                    if ((center - new Vec3(4, 0.2, 0)).Length > 0.9)
                    {
                        Material sphereMaterial;

                        if (chooseMat < 0.8)
                        {
                            // diffuse
                            var albedo = Util.RandomRgb() * Util.RandomRgb();
                            sphereMaterial = new Lambertian(albedo);
                            var center2 = center + new Vec3(0, Util.Rand() / 2, 0);
                            world.Add(new MovingSphere(center, 0, center2, 1.0, 0.2, sphereMaterial));
                        }
                        else if (chooseMat < 0.95)
                        {
                            // metal
                            var albedo = Util.RandomRgb(0.5, 1);
                            var fuzz = Util.Rand(0, 0.5);
                            sphereMaterial = new Metal(albedo, fuzz);
                            world.Add(new Sphere(center, 0.2, sphereMaterial));
                        }
                        else
                        {
                            // glass
                            sphereMaterial = new Dielectric(1.5);
                            world.Add(new Sphere(center, 0.2, sphereMaterial));
                        }
                    }
                }
            }

            var material1 = new Dielectric(1.5);
            world.Add(new Sphere(new Vec3(0, 1, 0), 1.0, material1));

            var material2 = new Lambertian(new Rgb(0.4, 0.2, 0.1));
            world.Add(new Sphere(new Vec3(-4, 1, 0), 1.0, material2));

            var material3 = new Metal(new Rgb(0.7, 0.6, 0.5), 0.0);
            world.Add(new Sphere(new Vec3(4, 1, 0), 1.0, material3));

            return world;
        }

        /// <summary>
        /// 地面となる非常に大きな球と、三つの大きな球と、多数の小さな球を生成する
        /// </summary>
        /// <returns></returns>
        static List<Hittable> CreateScene1()
        {
            var hittables = new List<Hittable>();
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
                        var center2 = center + new Vec3(0, Rand() * 0.5, 0);
                        hittables.Add(new MovingSphere(
                            center, 0.0, center2, 1.0, 0.2, material));
                    }
                    else if (chooseMat < 0.95)
                    {
                        // 金属
                        material = new Metal(new Rgb(0.5 * (1 + Rand()), 0.5 * (1 + Rand()), 0.5 * (1 + Rand())),
                                0.5 * Rand());
                        hittables.Add(new Sphere(center, 0.2, material));
                    }
                    else
                    {
                        // ガラス
                        material = new Dielectric(1.5);
                        hittables.Add(new Sphere(center, 0.2, material));
                    }
                }
            }

            // 三つの大きな球
            hittables.Add(new Sphere(new Vec3(0.0, 1.0, 0.0), 1.0, new Dielectric(1.5)));
            hittables.Add(new Sphere(new Vec3(-4.0, 1.0, 0.0), 1.0, new Lambertian(new Rgb(0.4, 0.2, 0.1))));
            hittables.Add(new Sphere(new Vec3(4.0, 1.0, 0.0), 1.0, new Metal(new Rgb(0.7, 0.6, 0.5), 0.0)));

            // 地面となる非常に大きな球
            hittables.Add(new Sphere(new Vec3(0.0, -1000.0, 0.0), 1000.0, new Lambertian(new Rgb(0.5, 0.5, 0.5))));
            return hittables;
        }

        static List<Hittable> CreateScene2()
        {
            var hittables = new List<Hittable>();

            var checker = new CheckerTexture(new Rgb(0.2, 0.3, 0.1), new Rgb(0.9, 0.9, 0.9));
            hittables.Add(new Sphere(new Vec3(0, -10, 0), 10, new Lambertian(checker)));
            hittables.Add(new Sphere(new Vec3(0, 10, 0), 10, new Lambertian(checker)));

            return hittables;
        }
    }
}
