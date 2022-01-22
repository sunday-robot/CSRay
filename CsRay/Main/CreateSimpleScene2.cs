using CsRay.Hittables;
using CsRay.Materials;

namespace CsRay.Main
{
    public partial class Program
    {
        /// <summary>
        /// 世界の中心に、半径1のグレイの球のみがあるシーンを生成する。
        /// </summary>
        static (List<Hittable>, Camera, Rgb?) CreateSimpleScene2()
        {
            var hittables = new List<Hittable>
            {
                new Sphere(new Vec3(-2, 0, 0), 1, new Lambertian(new Rgb(1, 0, 0))),
                new Sphere(new Vec3(0, 0, 0), 1, new Lambertian(new Rgb(0, 1, 0))),
                new Sphere(new Vec3(2, 0, 0), 1, new Lambertian(new Rgb(0, 0, 1)))
            };

            Camera camera;
            {
                var lookFrom = new Vec3(3, 2, 10);
                var lookAt = new Vec3(0, 0, 0);
                var vFov = 40;
                var aperture = 0.1F;
                var distanceToFocus = (lookAt - lookFrom).Length;
                var exposureTime = 1;
                camera = Camera.CreateCamera(lookFrom, lookAt, new Vec3(0, 1, 0), vFov, 16.0F / 9, aperture, distanceToFocus, exposureTime);
            }

            return (hittables, camera, null);
        }
    }
}
