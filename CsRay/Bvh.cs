using CsRay.Materials;

namespace CsRay
{
    public abstract class Bvh : Hittable
    {
        protected static bool DebugMode { get; set; } = false;
        protected static readonly Vec3 _dummyVec3 = new(0, 0, 0);

        protected readonly Aabb _aabb;
        protected readonly DebugMaterial _debugMaterial = new();

        public Bvh(Aabb aabb)
        {
            _aabb = aabb;
        }

        public sealed override Aabb BoundingBox(float exposureTime) => _aabb;

        public abstract void Print(string indent = "");
    }
}
