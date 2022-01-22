using CsRay.Hittables;
using CsRay.Materials;

namespace CsRay.Main
{
    public partial class Program
    {
        static (List<Hittable>, Camera, Rgb) CreateSingleBoxScene()
        {
            var objects = new List<Hittable>();
            {
                var red = new Lambertian(0.8, 0.2, 0.2);
                var box = new Box(new Vec3(-1, -1, -1), new Vec3(1, 1, 1), red);
                objects.Add(box);

#if false
                {
                    var blue = new Lambertian(0.2, 0.2, 0.8);
                    var sphere1 = new Sphere(new Vec3(-2, 0, 0), 1, blue);
                    objects.Add(sphere1);

                    var sphere2 = new Sphere(new Vec3(2, 0, 0), 1, blue);
                    objects.Add(sphere2);
                }
#endif
            }

            Camera camera;
            {
                var lookFrom = new Vec3(5, 3, 13);
                var lookAt = new Vec3(0, 0, 0);
                var vFov = 20;
                var aperture = 0.1;
                var distanceToFocus = (lookAt - lookFrom).Length;
                var exposureTime = 1;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0, 1, 0), vFov, 16.0 / 9, aperture, distanceToFocus, exposureTime);
            }

#if false
            return (objects, camera, null);
#else
            objects.Add(new Sphere(new Vec3(0, 5, 5), 1, new DiffuseLight(new Rgb(10, 10, 10))));
            return (objects, camera, Rgb.Black);
#endif
        }
    }
}
