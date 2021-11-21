using System.Collections.Generic;

namespace CsRay
{
    /// <summary>
    /// [Ray]と[Hitable]の衝突情報
    /// </summary>
    public sealed class HitableList
    {
        readonly List<Hittable> _list;

        public HitableList(List<Hittable> list)
        {
            _list = list;
        }

        public HitRecord Hit(Ray r, double tMin, double tMax, HitRecord rec)
        {
            HitRecord hitRecord = null;
            var closestSoFar = tMax;
            foreach (var hitable in _list)
            {
                var tmp = hitable.Hit(r, tMin, closestSoFar, rec);
                if (tmp != null)
                {
                    closestSoFar = tmp.T;
                    hitRecord = tmp;
                }
            }
            return hitRecord;
        }

        public Aabb BoundingBox(double t0, double t1)
        {
            if (_list.Count == 0)
                return null;
            var box = _list[0].BoundingBox(t0, t1);
            for (var i = 0; i < _list.Count; i++)
            {
                box = Aabb.SurroundingAabb(box, _list[i].BoundingBox(t0, t1));
            }
            return box;
        }
    }
}
