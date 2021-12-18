namespace CsRay.Hittables
{
    public sealed class XyRect : Hittable
    {
        readonly double _x0;
        readonly double _y0;
        readonly double _x1;
        readonly double _y1;
        readonly double _k;
        readonly Material _mp;

        public XyRect(double x0, double y0, double x1, double y1, double k, Material material)
        {
            _x0 = x0;
            _y0 = y0;
            _x1 = x1;
            _y1 = y1;
            _k = k;
            _mp = material;
        }

        public override Aabb BoundingBox(double t0, double t1)
        {
            // The bounding box must have non-zero width in each dimension, so pad the Z
            // dimension a small amount.
            return new Aabb(new Vec3(_x0, _y0, _k - 0.0001), new Vec3(_x1, _y1, _k + 0.0001));
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            var t = (_k - ray.Origin.Z) / ray.Direction.Z;
            if (t < tMin || t > tMax)
                return false;

            var x = ray.Origin.X + t * ray.Direction.X;
            var y = ray.Origin.Y + t * ray.Direction.Y;
            if (x < _x0 || x > _x1 || y < _y0 || y > _y1)
                return false;

            rec.SetUv((x - _x0) / (_x1 - _x0), (y - _y0) / (_y1 - _y0));
            rec.SetT(t);
            var outwardNormal = new Vec3(0, 0, 1);
            rec.SetFaceNormal(ray, outwardNormal);
            rec.SetMaterial(_mp);
            rec.SetPosition(ray.PositionAt(t));

            return true;
        }
    }
}
