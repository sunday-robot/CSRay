using CsRay.Hittables;
using CsRay.Materials;
using CsRay.Textures;

namespace CsRay.Main
{
    public partial class Program
    {
        static (List<Hittable>, Camera, Rgb) CreateSimpleLight()
        {
            var hittables = new List<Hittable>();
            {
                var pertext = new NoiseTexture(4);
                hittables.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(pertext)));
                hittables.Add(new Sphere(new Vec3(0, 2, 0), 2, new Lambertian(pertext)));

                var difflight = new DiffuseLight(new SolidColor(4, 4, 4));
                hittables.Add(new Sphere(new Vec3(0, 7, 0), 2, difflight));
                hittables.Add(new XyRect(3, 5, 1, 3, -2, difflight));
            }

            Camera camera;
            {
                var lookFrom = new Vec3(26, 3, 6);
                var lookAt = new Vec3(0, 2, 0);
                var vFov = 20;
                var aperture = 0.1;
                var distanceToFocus = (lookAt - lookFrom).Length;
                var exposureTime = 1;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0, 1, 0), vFov, 16.0 / 9, aperture, distanceToFocus, exposureTime);
            }

            return (hittables, camera, Rgb.Black);
        }
    }
}
