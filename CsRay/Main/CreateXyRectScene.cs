using CsRay.Hittables;
using CsRay.Materials;
using System.Collections.Generic;

namespace CsRay.Main
{
    public partial class Program
    {
        static (List<Hittable>, Camera, Rgb) CreateXyRectScene()
        {
            var objects = new List<Hittable>();
            {
                var red = new Lambertian(0.8, 0.2, 0.2);
                var rect = new XyRect(-1, -1, 1, 1, 0, red);
                objects.Add(rect);
            }

            Camera camera;
            {
                var lookFrom = new Vec3(5.0, 3.0, 13.0);
                var lookAt = new Vec3(0.0, 0.0, 0.0);
                var vFov = 20.0;
                var aperture = 0.1;
                var distanceToFocus = (lookAt - lookFrom).Length;
                var exposureTime = 1.0;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0.0, 1.0, 0.0), vFov, 16.0 / 9, aperture, distanceToFocus, exposureTime);
            }

            return (objects, camera, null);
        }
    }
}
