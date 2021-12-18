namespace CsRay.Hittables
{
    public sealed class XzRect : Hittable
    {
        readonly double _x0;
        readonly double _z0;
        readonly double _x1;
        readonly double _z1;
        readonly double _k;
        readonly Material _mp;

        public XzRect(double x0, double z0, double x1, double z1, double k, Material materal)
        {
            _x0 = x0;
            _z0 = z0;
            _x1 = x1;
            _z1 = z1;
            _k = k;
            _mp = materal;
        }

        public override Aabb BoundingBox(double t0, double t1)
        {
            // The bounding box must have non-zero width in each dimension, so pad the X
            // dimension a small amount.
            return new Aabb(new Vec3(_x0, _k - 0.0001, _z0), new Vec3(_x1, _k + 0.0001, _z1));
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            var t = (_k - ray.Origin.Y) / ray.Direction.Y;
            if (t < tMin || t > tMax)
                return false;

            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;
            if (x < _x0 || x > _x1 || z < _z0 || z > _z1)
                return false;

            rec.SetUv((x - _x0) / (_x1 - _x0), (z - _z0) / (_z1 - _z0));
            rec.SetT(t);
            var outwardNormal = new Vec3(0, 1, 0);
            rec.SetFaceNormal(ray, outwardNormal);
            rec.SetMaterial(_mp);
            rec.SetPosition(ray.PositionAt(t));

            return true;
        }
    }
}
