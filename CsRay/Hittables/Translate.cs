namespace CsRay.Hittables
{
    public class Translate : Hittable
    {
        readonly Hittable _ptr;

        readonly Vec3 _offset;

        public Translate(Hittable p, Vec3 displacement)
        {
            _ptr = p;
            _offset = displacement;
        }

        public override HitRecord Hit(Ray ray, double tMin, double tMax)
        {
            var movedR = new Ray(ray.Origin - _offset, ray.Direction, ray.Time);
            var tmpRec = _ptr.Hit(movedR, tMin, tMax);
            if (tmpRec == null)
                return null;

            var p = tmpRec.Position + _offset;
            var ff = movedR.Direction.Dot(tmpRec.Normal) < 0;
            var n = tmpRec.Normal;
            if (!ff)
                n = -n;
            return new HitRecord(tmpRec.T, p, n, tmpRec.Material, ff);
        }

        public override Aabb BoundingBox(double dt)
        {
            var b = _ptr.BoundingBox(dt);
            if (b == null)
                return null;

            return new Aabb(
                b.Min + _offset,
                b.Max + _offset);
        }
    }
}
