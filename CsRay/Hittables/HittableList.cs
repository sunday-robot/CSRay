using System;
using System.Collections.Generic;

namespace CsRay.Hittables
{
    /// <summary>
    /// [Ray]と[Hittable]の衝突情報
    /// </summary>
    public sealed class HittableList : Hittable
    {
        readonly Hittable[] _objects;

        public HittableList(Hittable[] hittableArray)
        {
            _objects = new Hittable[hittableArray.Length];
            hittableArray.CopyTo(_objects, 0);
        }

        public HittableList(List<Hittable> hittableList)
        {
            _objects = hittableList.ToArray();
        }

        public override bool Hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            var tempRec = new HitRecord(0, new Vec3(0, 0, 0), new Vec3(0, 0, 0), null);
            var hitAnything = false;
            var closestSoFar = tMax;

            foreach (var hittable in _objects)
            {
                if (hittable.Hit(r, tMin, closestSoFar, ref tempRec))
                {
                    hitAnything = true;
                    closestSoFar = tempRec.T;
                    rec = tempRec;
                }
            }
            return hitAnything;
        }

        public override Aabb BoundingBox(double t0, double t1)
        {
            var box = _objects[0].BoundingBox(t0, t1);
            for (var i = 1; i < _objects.Length; i++)
            {
                box = Aabb.SurroundingAabb(box, _objects[i].BoundingBox(t0, t1));
            }
            return box;
        }
    }
}
