namespace CsRay
{
    public static class CreateBvhTree
    {
        public static Bvh Create(List<Hittable> objects, double exposureTime)
        {
            return Create(objects, exposureTime, 0, objects.Count);
        }

        static Bvh Create(List<Hittable> objects, double exposureTime, int start, int end)
        {
            var object_span = end - start;

            if (object_span <= 0)
            {
                throw new ArgumentException("empty list.");
            }

            if (object_span == 1)
            {
                var o = objects[start];
                return new BvhLeaf(o.BoundingBox(exposureTime), o);
            }
            else
            {
                var comparator = GetComparerRandomly();
                objects.Sort(start, end - start, comparator);
                var mid = (start + end) / 2;
                var left = Create(objects, exposureTime, start, mid);
                var right = Create(objects, exposureTime, mid, end);
                var aabb = Aabb.SurroundingAabb(left.BoundingBox(exposureTime), right.BoundingBox(exposureTime));
                return new BvhNode(aabb, left, right);
            }
        }

        static readonly IComparer<Hittable> _boxCompareX = Comparer<Hittable>.Create((a, b) =>
        {
            BoxCompareSub(a, b, out var boxA, out var boxB);
            return boxB.Min.X.CompareTo(boxA.Min.X);
        });

        static readonly IComparer<Hittable> _boxCompareY = Comparer<Hittable>.Create((a, b) =>
        {
            BoxCompareSub(a, b, out var boxA, out var boxB);
            return boxB.Min.Y.CompareTo(boxA.Min.Y);
        });

        static readonly IComparer<Hittable> _boxCompareZ = Comparer<Hittable>.Create((a, b) =>
        {
            BoxCompareSub(a, b, out var boxA, out var boxB);
            return boxB.Min.Z.CompareTo(boxA.Min.Z);
        });

        static IComparer<Hittable> GetComparerRandomly()
        {
            var axis = Util.RandInt() % 3;
            return axis switch
            {
                0 => _boxCompareX,
                1 => _boxCompareY,
                _ => _boxCompareZ,
            };
        }

        static void BoxCompareSub(Hittable a, Hittable b, out Aabb boxA, out Aabb boxB)
        {
            boxA = a.BoundingBox(0);
            boxB = b.BoundingBox(0);
        }
    }
}
