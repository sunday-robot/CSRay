using CsRay.Hittables;
using CsRay.Materials;
using CsRay.Textures;
using System.ComponentModel;

namespace CsRay.Main
{
    public partial class Program
    {
        static (List<Hittable>, Camera, Rgb) CreateFinalScene()
        {
            var objects = new List<Hittable>();
            {
                // 床に敷き詰められた淡い緑の箱
#if true
                var boxes1 = new List<Hittable>();
                {
                    var material = new Lambertian(0.48, 0.83, 0.53);
                    var boxesPerSide = 20;
                    var w = 100;
                    for (int i = 0; i < boxesPerSide; i++)
                    {
                        for (int j = 0; j < boxesPerSide; j++)
                        {
                            var x0 = -1000 + i * w;
                            var z0 = -1000 + j * w;
                            var y0 = 0;
                            var x1 = x0 + w;
                            var y1 = 1 + Rand() * 100;
                            var z1 = z0 + w;

                            boxes1.Add(new Box(new Vec3(x0, y0, z0), new Vec3(x1, y1, z1), material));
                        }
                    }
                }
                objects.Add(CreateBvhTree.Create(boxes1, 1));
#endif
                // 天井の白い四角い照明
#if true
                var light = new DiffuseLight(7, 7, 7);
                objects.Add(new XzRect(123, 147, 423, 412, 554, light));
#endif
                // 画面左上の移動中のオレンジ色の球
#if true
                var center = new Vec3(400, 400, 200);
                var velocity = new Vec3(30, 0, 0);
                var movingSphereMaterial = new Lambertian(0.7, 0.3, 0.1);
                objects.Add(new MovingSphere(center, 50, movingSphereMaterial, velocity));
#endif
                // 画面中央下部のガラス玉
#if true
                objects.Add(new Sphere(new Vec3(260, 150, 45), 50, new Dielectric(1.5)));
#endif
                // 画面右下の銀色の玉
#if true
                objects.Add(new Sphere(new Vec3(0, 150, 145), 50, new Metal(new Rgb(0.8, 0.8, 0.9), 1)));
#endif

                // 画面左下のガラス玉？
#if true
                {
                    var boundary = new Sphere(new Vec3(360, 150, 145), 70, new Dielectric(1.5));
                    objects.Add(boundary);
                    objects.Add(new ConstantMedium(boundary, 0.2, new Rgb(0.2, 0.4, 0.9)));
                }
#endif
                // シーン全体を覆う霧
#if true
                {
                    var boundary = new Sphere(new Vec3(0, 0, 0), 5000, new Dielectric(1.5));
                    objects.Add(new ConstantMedium(boundary, .0001, new Rgb(1, 1, 1)));
                }
#endif
                // 画面左の地球
#if true
                var emat = new Lambertian(new ImageTexture("../../../earthmap.bmp"));
                objects.Add(new Sphere(new Vec3(400, 200, 400), 100, emat));
#endif
                // 画面中央の白い球
#if true
                var pertext = new NoiseTexture(0.1);
                objects.Add(new Sphere(new Vec3(220, 280, 300), 80, new Lambertian(pertext)));
#endif
                // 画面左上の小さな球の集団
#if true
                var boxes2 = new List<Hittable>();
                var white = new Lambertian(.73, .73, .73);
                int ns = 1000;
                for (int j = 0; j < ns; j++)
                {
                    boxes2.Add(new Sphere(new Vec3(Random.Shared.NextDouble(), Random.Shared.NextDouble(), Random.Shared.NextDouble()) * 165, 10, white));
                }
                objects.Add(new Translate(new RotateY(CreateBvhTree.Create(boxes2, 1), 15), new Vec3(-100, 270, 395)));
#else
#if true
                {
                    const int sphereCount = 3;
                    const double sphereRadius = 50;
                    var boxes2 = new List<Hittable>();
                    for (int i = 0; i < sphereCount; i++)
                    {
                        for (int j = 0; j < sphereCount; j++)
                        {
                            for (int k = 0; k < sphereCount; k++)
                            {
                                var material = new Lambertian(
                                    i / (sphereCount - 1.0),
                                    j / (sphereCount - 1.0),
                                    k / (sphereCount - 1.0));
                                var sphere = new Sphere(
                                    new Vec3(i * sphereRadius * 2, j * sphereRadius * 2, k * sphereRadius * 2),
                                    sphereRadius,
                                    material);
                                boxes2.Add(sphere);
                            }
                        }
                    }
                    Hittable hittable;
                    hittable = CreateBvhTree.Create(boxes2, 1);
                    hittable = new RotateY(hittable, 15);
                    hittable = new Translate(hittable, new Vec3(-100, 270, 395));
                    objects.Add(hittable);
                }
#else
                {
                    const double sphereRadius = 100;
                    var o = new Vec3(278, 278, 400);
                    Material material;
                    Vec3 vec3;

                    vec3 = new Vec3(-sphereRadius * 2, 0, 0);
                    material = new Lambertian(1, 0, 0);
                    objects.Add(new Sphere(o + vec3, sphereRadius, material));

                    vec3 = new Vec3(0, 0, 0);
                    material = new Lambertian(0, 1, 0);
                    objects.Add(new Sphere(o + vec3, sphereRadius, material));

                    vec3 = new Vec3(sphereRadius * 2, 0, 0);
                    material = new Lambertian(0, 0, 1);
                    objects.Add(new Sphere(o + vec3, sphereRadius, material));

                    material = new Lambertian(1, 1, 1);
                    vec3 = new Vec3(0, sphereRadius * 2, 0);
                    objects.Add(new Sphere(o + vec3, sphereRadius, material));
                    vec3 = new Vec3(0, -sphereRadius * 2, 0);
                    objects.Add(new Sphere(o + vec3, sphereRadius, material));
                }
#endif
#endif

                // C++版でのRotateYのバグ調査用の立方体
#if false
                {
                    var material = new Lambertian(0.48, 0.83, 0.53);
                    Hittable hittable = new Box(new Vec3(0, 0, 0), new Vec3(100, 100, 100), material);
                    //hittable = new RotateY(hittable, 15);
                    //hittable = new RotateY(hittable, 0);
                    hittable = new RotateY(hittable, 45);
                    objects.Add(hittable);
                }
#endif
            }

            Camera camera;
            {
                var lookFrom = new Vec3(478, 278, -600);
                var lookAt = new Vec3(278, 278, 0);
                var vFov = 40;
                var aperture = 0;
                var exposureTime = 1;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0, 1, 0), vFov, 16.0 / 9, aperture, exposureTime);
            }
            return (objects, camera, Rgb.Black);
        }
    }
}
