using System.Collections.Generic;

namespace CsRay
{
    /// <summary>
    /// [Ray]と[Hittable]の衝突情報
    /// </summary>
    public sealed class HittableList : Hittable
    {
        public List<Hittable> Objects { get; }

        public HittableList(List<Hittable> list)
        {
            Objects = list;
        }

        public override bool Hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            var tempRec = new HitRecord(0, new Vec3(0, 0, 0), new Vec3(0, 0, 0), null);
            var hitAnything = false;
            var closestSoFar = tMax;

            foreach (var hittable in Objects)
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
            if (Objects.Count == 0)
                return null;
            var box = Objects[0].BoundingBox(t0, t1);
            for (var i = 1; i < Objects.Count; i++)
            {
                box = Aabb.SurroundingAabb(box, Objects[i].BoundingBox(t0, t1));
            }
            return box;
        }
    }
}
