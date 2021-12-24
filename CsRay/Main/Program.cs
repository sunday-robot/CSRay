using CsRay.Hittables;
using System;
using System.Diagnostics;

namespace CsRay.Main
{
    public partial class Program
    {
        static readonly Random _rand = new Random();
        static double Rand() => _rand.NextDouble();
        static double Rand2() => _rand.NextDouble() * _rand.NextDouble();

        static void Main()
        {
#if true
            var imageWidth = 1280;
            var imageHeight = 720;
            var overSamplingCount = 10;
            //var overSamplingCount = 100;
#else
            var imageWidth = 320;
            var imageHeight = 180;
            var overSamplingCount = 10;
#endif
            //var (hittables, camera, background) = CreateXyRectScene();
            var (hittables, camera, background) = CreateSingleBoxScene();
            //var (hittables, camera, background) = CreateCornellSmoke();
            //var (hittables, camera, background) = CreateFinalScene();
            //var (hittables, camera, background) = CreateCornellBox();
            //var (hittables, camera, background) = CreateSimpleLight();
            //var (hittables, camera, background) = CreateSimpleScene();
            //var (hittables, camera, background) = CreateSimpleScene2();
            //var (hittables, camera, background) = CreateScene0();
            //var (hittables, camera, background) = CreateScene1();
            //var (hittables, camera, background) = CreateScene2();
            //var (hittables, camera, background) = Create2PerlinSpheresScene();
            //var (hittables, camera, background) = CreateRandomScene();
#if false
            var world = new BvhNode(hittables, 0, 1);
            world.Print();
#else
            var world = new HittableList(hittables);
#endif
#if true
            BvhNode.DebugMode = true;
#endif
            var pixels = Renderer.Render(world, camera, imageWidth, imageHeight, overSamplingCount, background);

            try
            {
                Util.SaveAsPpm(imageWidth, imageHeight, pixels, "Spheres.ppm");
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Debug.WriteLine(e);
            }
        }
    }
}
