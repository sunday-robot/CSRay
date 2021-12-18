using CsRay.Hittables;
using CsRay.Materials;
using CsRay.Textures;
using System.Collections.Generic;

namespace CsRay.Main
{
    public partial class Program
    {
        static (List<Hittable>, Camera, Rgb) CreateScene2()
        {
            var hittables = new List<Hittable>();
            {
                var checker = new CheckerTexture(new Rgb(0.2, 0.3, 0.1), new Rgb(0.9, 0.9, 0.9));
                hittables.Add(new Sphere(new Vec3(0, -10, 0), 10, new Lambertian(checker)));
                hittables.Add(new Sphere(new Vec3(0, 10, 0), 10, new Lambertian(checker)));
            }

            Camera camera;
            {
                var lookFrom = new Vec3(13.0, 2.0, 3.0);
                var lookAt = new Vec3(0.0, 0.0, 0.0);
                var vFov = 20.0;
                var aperture = 0.1;
                var distanceToFocus = (lookAt - lookFrom).Length;
                var time0 = 0.0;
                var time1 = 1.0;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0.0, 1.0, 0.0), vFov, 16.0 / 9, aperture, distanceToFocus, time0, time1);
            }

            return (hittables, camera, null);
        }
    }
}
