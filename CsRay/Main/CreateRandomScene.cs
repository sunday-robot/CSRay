using CsRay.Hittables;
using CsRay.Materials;
using CsRay.Textures;

namespace CsRay.Main
{
    public partial class Program
    {
        static (List<Hittable>, Camera, Rgb?) CreateRandomScene()
        {
            var world = new List<Hittable>();
            {
                var checker = new CheckerTexture(new Rgb(0.2F, 0.3F, 0.1F), new Rgb(0.9F, 0.9F, 0.9F));

                world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(checker)));

                for (int a = -11; a < 11; a++)
                {
                    for (int b = -11; b < 11; b++)
                    {
                        var chooseMat = Rand();
                        var center = new Vec3(a + 0.9F * Rand(), 0.2F, b + 0.9F * Rand());

                        if ((center - new Vec3(4, 0.2F, 0)).Length > 0.9F)
                        {
                            Material sphereMaterial;

                            if (chooseMat < 0.8)
                            {
                                // diffuse
                                var albedo = Util.RandomRgb() * Util.RandomRgb();
                                sphereMaterial = new Lambertian(albedo);
                                var velocity = new Vec3(0, Rand() / 2, 0);
                                world.Add(new MovingSphere(center, 0.2F, sphereMaterial, velocity));
                            }
                            else if (chooseMat < 0.95)
                            {
                                // metal
                                var albedo = Util.RandomRgb(0.5F, 1);
                                var fuzz = Rand() * 0.5F;
                                sphereMaterial = new Metal(albedo, fuzz);
                                world.Add(new Sphere(center, 0.2F, sphereMaterial));
                            }
                            else
                            {
                                // glass
                                sphereMaterial = new Dielectric(1.5F);
                                world.Add(new Sphere(center, 0.2F, sphereMaterial));
                            }
                        }
                    }
                }

                var material1 = new Dielectric(1.5F);
                world.Add(new Sphere(new Vec3(0, 1, 0), 1, material1));

                var material2 = new Lambertian(new Rgb(0.4F, 0.2F, 0.1F));
                world.Add(new Sphere(new Vec3(-4, 1, 0), 1, material2));

                var material3 = new Metal(new Rgb(0.7F, 0.6F, 0.5F), 0);
                world.Add(new Sphere(new Vec3(4, 1, 0), 1, material3));
            }

            Camera camera;
            {
                var lookFrom = new Vec3(13, 2, 3);
                var lookAt = new Vec3(0, 0, 0);
                var vFov = 20;
                var aperture = 0.1F;
                var distanceToFocus = (lookAt - lookFrom).Length;
                var exposureTime = 1;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0, 1, 0), vFov, 16F / 9, aperture, distanceToFocus, exposureTime);
            }

            return (world, camera, null);
        }
    }
}
