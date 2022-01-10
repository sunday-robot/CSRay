using CsRay.Materials;
using System.Diagnostics;

namespace CsRay.Hittables
{
    public sealed class BvhNode : Hittable
    {
        public static bool DebugMode { get; set; } = false;
        static readonly Vec3 _dummyVec3 = new(0, 0, 0);

        readonly Hittable _left;
        readonly Hittable? _right;
        readonly Aabb _aabb;
        readonly DebugMaterial _debugMaterial = new();

        public BvhNode(List<Hittable> list, double dt) :
            this(new List<Hittable>(list), 0, list.Count, dt)
        { }

        BvhNode(List<Hittable> objects, int start, int end, double dt)
        {
            var object_span = end - start;

            if (object_span <= 0)
            {
                throw new ArgumentException("empty list.");
            }

            if (object_span == 1)
            {
                _left = objects[start];
                _right = null;
                _aabb = _left.BoundingBox(dt);
            }
            else
            {
                var comparator = GetComparerRandomly();

                if (object_span == 2)
                {
                    if (comparator.Compare(objects[start], objects[start + 1]) > 0)
                    {
                        _left = objects[start];
                        _right = objects[start + 1];
                    }
                    else
                    {
                        _left = objects[start + 1];
                        _right = objects[start];
                    }
                }
                else
                {
                    objects.Sort(start, end - start, comparator);
                    var mid = start + object_span / 2;
                    _left = new BvhNode(objects, start, mid, dt);
                    _right = new BvhNode(objects, mid, end, dt);
                }

                var boxLeft = _left.BoundingBox(dt);
                var boxRight = _right.BoundingBox(dt);
                _aabb = Aabb.SurroundingAabb(boxLeft, boxRight);
            }
        }

        public override HitRecord? Hit(Ray ray, double tMin, double tMax)
        {
            if (!_aabb.Hit(ray, tMin, tMax))
                return null;

            if (DebugMode)
            {
                if (!(_left is BvhNode) && !(_right is BvhNode))
                    return new HitRecord(0, _dummyVec3, _dummyVec3, _debugMaterial);
            }

            var rec1 = _left.Hit(ray, tMin, tMax);
            if (rec1 != null)
            {
                if (_right != null)
                {
                    var rec2 = _right.Hit(ray, tMin, rec1.T);
                    if (rec2 != null)
                        return rec2;
                }
                return rec1;
            }
            else
            {
                if (_right == null)
                    return null;
                return _right.Hit(ray, tMin, tMax);
            }
        }

        public override Aabb BoundingBox(double dt) => _aabb;

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
            if (boxA == null || boxB == null)
                Debug.WriteLine($"No bounding box in bvh_node constructor.");
        }

        public void Print(string indent = "")
        {
            Console.WriteLine($"{indent}aabb = {_aabb}");
            if (_right == null)
                Console.WriteLine($"{indent}{_left}");
            else
            {
                if (_left is BvhNode leftBhvNode)
                {
                    Console.WriteLine($"{indent}left:");
                    leftBhvNode.Print(indent + "  ");
                }
                else
                    Console.WriteLine($"{indent}left = {_left}");
                if (_right is BvhNode rightBhvNode)
                {
                    Console.WriteLine($"{indent}right:");
                    rightBhvNode.Print(indent + "  ");
                }
                else
                    Console.WriteLine($"{indent}right = {_right}");
            }
        }
    }
}
