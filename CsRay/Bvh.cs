using CsRay.Materials;

namespace CsRay
{
    public abstract class Bvh : Hittable
    {
        protected static bool DebugMode { get; set; } = false;

        protected readonly Aabb _aabb;
        protected readonly DebugMaterial _debugMaterial = new();

        public Bvh(Aabb aabb)
        {
            _aabb = aabb;
        }

        public sealed override Aabb BoundingBox(double exposureTime) => _aabb;

        public abstract void Print(string indent = "");
    }
}
