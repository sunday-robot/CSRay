namespace CsRay
{
    public sealed class BvhLeaf : Bvh
    {
        readonly Hittable _hittable;

        public BvhLeaf(Aabb aabb, Hittable hittable) : base(aabb)
        {
            _hittable = hittable;
        }

        public override HitRecord? Hit(Ray ray, double tMin, double tMax)
        {
            if (!_aabb.Hit(ray, tMin, tMax))
                return null;

            if (DebugMode)
            {
                return new HitRecord(0, _dummyVec3, _dummyVec3, _debugMaterial);
            }

            return _hittable.Hit(ray, tMin, tMax);
        }

        public override void Print(string indent = "")
        {
            Console.WriteLine($"{indent}aabb = {_aabb}");
            Console.WriteLine($"{indent}{_hittable}");
        }
    }
}
