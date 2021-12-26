using CsRay.Hittables;
using CsRay.Materials;
using System.Collections.Generic;

namespace CsRay.Main
{
    public partial class Program
    {
        static (List<Hittable>, Camera, Rgb) CreateCornellBox()
        {
            var objects = new List<Hittable>();
            {
                var red = new Lambertian(new Rgb(.65, .05, .05));
                var white = new Lambertian(new Rgb(.73, .73, .73));
                var green = new Lambertian(new Rgb(.12, .45, .15));
                var light = new DiffuseLight(new Rgb(15, 15, 15));

                objects.Add(new YzRect(0, 0, 555, 555, 555, green));
                objects.Add(new YzRect(0, 0, 555, 555, 0, red));
                objects.Add(new XzRect(213, 227, 343, 332, 554, light));
                objects.Add(new XzRect(0, 0, 555, 555, 555, white));
                objects.Add(new XzRect(0, 0, 555, 555, 0, white));
                objects.Add(new XyRect(0, 0, 555, 555, 555, white));

                Hittable box1;
                box1 = new Box(new Vec3(0, 0, 0), new Vec3(165, 330, 165), white);
                box1 = new RotateY(box1, 15);
                box1 = new Translate(box1, new Vec3(265, 0, 295));
                objects.Add(box1);

                Hittable box2;
                box2 = new Box(new Vec3(0, 0, 0), new Vec3(165, 165, 165), white);
                box2 = new RotateY(box2, -18);
                box2 = new Translate(box2, new Vec3(130, 0, 65));
                objects.Add(box2);
            }

            Camera camera;
            {
                var lookFrom = new Vec3(278, 278, -800);
                var lookAt = new Vec3(278, 278, 0);
                var vFov = 40.0;
                var aperture = 0.1;
                var distanceToFocus = (lookAt - lookFrom).Length;
                var exposureTime = 1.0;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0.0, 1.0, 0.0), vFov, 16.0 / 9, aperture, distanceToFocus, exposureTime);
            }

            return (objects, camera, Rgb.Black);
        }
    }
}
