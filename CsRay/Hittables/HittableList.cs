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

        public override HitRecord? Hit(Ray r, double tMin, double tMax)
        {
            var closestSoFar = tMax;
            HitRecord? rec = null;

            foreach (var hittable in _objects)
            {
                var tmpRec = hittable.Hit(r, tMin, closestSoFar);
                if (tmpRec != null)
                {
                    closestSoFar = tmpRec.T;
                    rec = tmpRec;
                }
            }
            return rec;
        }

        public override Aabb BoundingBox(double dt)
        {
            var box = _objects[0].BoundingBox(dt);
            for (var i = 1; i < _objects.Length; i++)
            {
                box = Aabb.SurroundingAabb(box, _objects[i].BoundingBox(dt));
            }
            return box;
        }
    }
}
