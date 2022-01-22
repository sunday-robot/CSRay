using CsRay.Hittables;
using CsRay.Materials;

namespace CsRay.Main
{
    public partial class Program
    {
        static (List<Hittable>, Camera, Rgb) CreateFinalScene2()
        {
            var objects = new List<Hittable>();
            {
#if false
                var material = new Lambertian(0.48, 0.83, 0.53);
                var boxesPerSide = 20;
                var w = 100.0;
                for (int i = 0; i < boxesPerSide; i++)
                {
                    for (int j = 0; j < boxesPerSide; j++)
                    {
                        var x0 = -1000.0 + i * w;
                        var z0 = -1000.0 + j * w;
                        var y0 = 0.0;
                        var x1 = x0 + w;
                        var y1 = 1 + Rand() * 100;
                        var z1 = z0 + w;

                        objects.Add(new Box(new Vec3(x0, y0, z0), new Vec3(x1, y1, z1), material));
                    }
                }
#endif

                // 天井の矩形の照明
                var light = new DiffuseLight(7, 7, 7);
                objects.Add(new XzRect(123, 147, 423, 412, 554, light));

#if false
                var center1 = new Vec3(400, 400, 200);
                var center2 = center1 + new Vec3(30, 0, 0);
                var movingSphereMaterial = new Lambertian(0.7, 0.3, 0.1);
                objects.Add(new MovingSphere(center1, center2, 0, 1, 50, movingSphereMaterial));
#endif

#if false
                objects.Add(new Sphere(new Vec3(260, 150, 45), 50, new Dielectric(1.5)));
                objects.Add(new Sphere(new Vec3(0, 150, 145), 50, new Metal(new Rgb(0.8, 0.8, 0.9), 1.0)));
#endif

#if false
                {
                    var boundary = new Sphere(new Vec3(360, 150, 145), 70, new Dielectric(1.5));
#if true
                    objects.Add(boundary);
#endif
#if true
                    objects.Add(new ConstantMedium(boundary, 0.2, new Rgb(0.2, 0.4, 0.9)));
#endif
                }
#endif

#if true
                {
                    // シーン全体を覆う気体のようなもの
                    var boundary = new Sphere(new Vec3(0, 0, 0), 5000, new Dielectric(1.5F));
                    objects.Add(new ConstantMedium(boundary, .0001F, new Rgb(1, 1, 1)));
                }
#endif

#if false
#if false
            var emat = new Lambertian(new ImageTexture("earthmap.jpg"));
#else
                var emat = new Lambertian(0.1, 0.2, 0.8);
#endif
                objects.Add(new Sphere(new Vec3(400, 200, 400), 100, emat));
                var pertext = new NoiseTexture(0.1);
                objects.Add(new Sphere(new Vec3(220, 280, 300), 80, new Lambertian(pertext)));
#endif

#if false
                var boxes2 = new List<Hittable>();
                var white = new Lambertian(.73, .73, .73);
                int ns = 1000;
                for (int j = 0; j < ns; j++)
                {
                    boxes2.Add(new Sphere(new Vec3(Util.Rand(), Util.Rand(), Util.Rand()) * 165, 10, white));
                }

                objects.Add(new Translate(
                    new RotateY(
                        new BvhNode(boxes2, 0.0, 1.0), 15),
                    new Vec3(-100, 270, 395)
                    )
                );
#endif
            }

            Camera camera;
            {
                var lookFrom = new Vec3(956, 556, -1200);
                var lookAt = new Vec3(278, 278, 0);
                var vFov = 40;
                var aperture = 0.1F;
                var distanceToFocus = (lookAt - lookFrom).Length;
                var exposureTime = 1;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0, 1, 0), vFov, 16F / 9, aperture, distanceToFocus, exposureTime);
            }

            return (objects, camera, Rgb.Black);
        }
    }
}
