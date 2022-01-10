namespace CsRay.Hittables
{
    public sealed class XzRect : Hittable
    {
        readonly double _x0;
        readonly double _z0;
        readonly double _x1;
        readonly double _z1;
        readonly double _k;
        readonly Material _material;

        readonly Aabb _aabb;

        public XzRect(double x0, double z0, double x1, double z1, double k, Material materal)
        {
            _x0 = x0;
            _z0 = z0;
            _x1 = x1;
            _z1 = z1;
            _k = k;
            _material = materal;

            // The bounding box must have non-zero width in each dimension, so pad the X
            // dimension a small amount.
            _aabb = new Aabb(new Vec3(x0, k - 0.0001, z0), new Vec3(x1, k + 0.0001, z1));
        }

        public override Aabb BoundingBox(double dt) => _aabb;

        public override HitRecord? Hit(Ray ray, double tMin, double tMax)
        {
            var t = (_k - ray.Origin.Y) / ray.Direction.Y;
            if (t < tMin || t > tMax)
                return null;

            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;
            if (x < _x0 || x > _x1 || z < _z0 || z > _z1)
                return null;

            var p = ray.PositionAt(t);
            var outwardNormal = new Vec3(0, 1, 0);
            var ff = ray.Direction.Dot(outwardNormal) < 0;
            var n = ff ? outwardNormal : -outwardNormal;
            var u = (x - _x0) / (_x1 - _x0);
            var v = (z - _z0) / (_z1 - _z0);
            return new HitRecord(t, p, n, _material, ff, u, v);
        }
    }
}
