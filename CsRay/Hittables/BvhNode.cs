using CsRay.Materials;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CsRay.Hittables
{
    public sealed class BvhNode : Hittable
    {
        readonly Hittable _left;
        readonly Hittable _right;
        readonly Aabb _aabb;
        readonly DebugMaterial _debugMaterial = new DebugMaterial();

        public BvhNode(List<Hittable> list, double time0, double time1) :
            this(new List<Hittable>(list), 0, list.Count, time0, time1)
        { }

        public BvhNode(List<Hittable> objects, int start, int end, double time0, double time1)
        {
            var object_span = end - start;

            if (object_span <= 0)
            {
                throw new ArgumentException("empty list.");
            }

            if (object_span == 1)
            {
                _left = _right = objects[start];
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
                    _left = new BvhNode(objects, start, mid, time0, time1);
                    _right = new BvhNode(objects, mid, end, time0, time1);
                }
            }

            var boxLeft = _left.BoundingBox(time0, time1);
            var boxRight = _right.BoundingBox(time0, time1);

            if (boxLeft == null || boxRight == null)
                Debug.WriteLine($"No bounding box in bvh_node constructor.");

            _aabb = Aabb.SurroundingAabb(boxLeft, boxRight);
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            if (!_aabb.Hit(ray, tMin, tMax))
                return false;
#if false
            if (!(_left is BvhNode) && !(_right is BvhNode))
            {
                rec.SetMaterial(_debugMaterial);
                return true;
            }
#endif
            var hitLeft = _left.Hit(ray, tMin, tMax, ref rec);
            if (hitLeft)
            {
                _right.Hit(ray, tMin, rec.T, ref rec);
                return true;
            }
            else
            {
                return _right.Hit(ray, tMin, tMax, ref rec);
            }
        }

        public override Aabb BoundingBox(double t0, double t1)
        {
            return _aabb;
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
            switch (axis)
            {
                case 0:
                    return _boxCompareX;
                case 1:
                    return _boxCompareY;
                default:
                    return _boxCompareZ;
            }
        }

        static void BoxCompareSub(Hittable a, Hittable b, out Aabb boxA, out Aabb boxB)
        {
            boxA = a.BoundingBox(0, 0);
            boxB = b.BoundingBox(0, 0);
            if (boxA == null || boxB == null)
                Debug.WriteLine($"No bounding box in bvh_node constructor.");
        }

        public void Print(string indent = "")
        {
            Console.WriteLine($"{indent}aabb = {_aabb}");
            if (_left == _right)
            {
                Console.WriteLine($"{indent}{_left}");
            }
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
