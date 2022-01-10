using CsRay.Hittables;
using System.Diagnostics;

namespace CsRay.Main
{
    public partial class Program
    {
        static readonly Random _rand = new();
        static double Rand() => _rand.NextDouble();
        static double Rand2() => _rand.NextDouble() * _rand.NextDouble();

        static void Main()
        {
#if true
            var imageWidth = 1280;
            var imageHeight = 720;
#else
            var imageWidth = 320;
            var imageHeight = 180;
#endif
#if false
            var overSamplingCount = 100;
#else
            var overSamplingCount = 10;
#endif
            //var (hittables, camera, background) = CreateXyRectScene();
            //var (hittables, camera, background) = CreateSingleBoxScene();
            //var (hittables, camera, background) = CreateCornellSmoke();
            var (hittables, camera, background) = CreateFinalScene();
            //var (hittables, camera, background) = CreateFinalScene2();
            //var (hittables, camera, background) = CreateCornellBox();
            //var (hittables, camera, background) = CreateSimpleLight();
            //var (hittables, camera, background) = CreateSimpleScene();
            //var (hittables, camera, background) = CreateSimpleScene2();
            //var (hittables, camera, background) = CreateScene0();
            //var (hittables, camera, background) = CreateScene1();
            //var (hittables, camera, background) = CreateScene2();
            //var (hittables, camera, background) = Create2PerlinSpheresScene();
            //var (hittables, camera, background) = CreateRandomScene();
#if true
            var world = new BvhNode(hittables, camera.ExposureTime);
            world.Print();
#else
            var world = new HittableList(hittables);
#endif
#if false
            BvhNode.DebugMode = false;
#endif
            var start = DateTime.UtcNow;
            var renderer = new Renderer(world, background);

            var pixels = renderer.Render(camera, imageWidth, imageHeight, 50, overSamplingCount);
            var end = DateTime.UtcNow;
            Console.WriteLine($"time = {end - start}");
            Console.ReadLine();

            try
            {
                Util.SaveAsBmp(imageWidth, imageHeight, pixels, "Spheres.bmp");
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Debug.WriteLine(e);
            }
        }
    }
}
