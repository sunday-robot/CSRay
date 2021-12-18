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

        public override bool Hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            var movedR = new Ray(r.Origin - _offset, r.Direction, r.Time);
            if (!_ptr.Hit(movedR, tMin, tMax, ref rec))
                return false;
            rec.ShiftPosition(_offset);
            rec.SetFaceNormal(movedR, rec.Normal);
            return true;
        }

        public override Aabb BoundingBox(double time0, double time1)
        {
            var b = _ptr.BoundingBox(time0, time1);
            if (b == null)
                return null;

            return new Aabb(
                b.Min + _offset,
                b.Max + _offset);
        }
    }
}
