using CsRay.Hittables;
using CsRay.Materials;
using CsRay.Textures;
using System.Collections.Generic;

namespace CsRay.Main
{
    public partial class Program
    {
        static (List<Hittable>, Camera, Rgb) CreateRandomScene()
        {
            var world = new List<Hittable>();
            {
                var checker = new CheckerTexture(new Rgb(0.2, 0.3, 0.1), new Rgb(0.9, 0.9, 0.9));

                world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(checker)));

                for (int a = -11; a < 11; a++)
                {
                    for (int b = -11; b < 11; b++)
                    {
                        var chooseMat = Rand();
                        var center = new Vec3(a + 0.9 * Rand(), 0.2, b + 0.9 * Rand());

                        if ((center - new Vec3(4, 0.2, 0)).Length > 0.9)
                        {
                            Material sphereMaterial;

                            if (chooseMat < 0.8)
                            {
                                // diffuse
                                var albedo = Util.RandomRgb() * Util.RandomRgb();
                                sphereMaterial = new Lambertian(albedo);
                                var velocity = new Vec3(0, Rand() / 2, 0);
                                world.Add(new MovingSphere(center, 0.2, sphereMaterial, velocity));
                            }
                            else if (chooseMat < 0.95)
                            {
                                // metal
                                var albedo = Util.RandomRgb(0.5, 1);
                                var fuzz = Rand() * 0.5;
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
            }

            Camera camera;
            {
                var lookFrom = new Vec3(13.0, 2.0, 3.0);
                var lookAt = new Vec3(0.0, 0.0, 0.0);
                var vFov = 20.0;
                var aperture = 0.1;
                var distanceToFocus = (lookAt - lookFrom).Length;
                var exposureTime = 1.0;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0.0, 1.0, 0.0), vFov, 16.0 / 9, aperture, distanceToFocus, exposureTime);
            }

            return (world, camera, null);
        }
    }
}
