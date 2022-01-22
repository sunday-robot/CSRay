namespace CsRay
{
    public sealed class BvhNode : Bvh
    {
        readonly Bvh _left;
        readonly Bvh _right;

        public BvhNode(Aabb aabb, Bvh left, Bvh right) : base(aabb)
        {
            _left = left;
            _right = right;
        }

        public override HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            if (!_aabb.Hit(ray, tMin, tMax))
                return null;

#if false
            if (DebugMode)
            {
                if (!(_left is BvhNode) && !(_right is BvhNode))
                    return new HitRecord(0, _dummyVec3, _dummyVec3, _debugMaterial);
            }
#endif

            var rec1 = _left.Hit(ray, tMin, tMax);
            if (rec1 != null)
            {
                var rec2 = _right.Hit(ray, tMin, rec1.T);
                if (rec2 != null)
                    return rec2;
                return rec1;
            }
            else
                return _right.Hit(ray, tMin, tMax);
        }

        public override void Print(string indent = "")
        {
            Console.WriteLine($"{indent}aabb = {_aabb}");
            Console.WriteLine($"{indent}left:");
            _left.Print(indent + "  ");
            Console.WriteLine($"{indent}right:");
            _right.Print(indent + "  ");
        }
    }
}
