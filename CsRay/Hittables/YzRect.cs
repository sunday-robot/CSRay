namespace CsRay.Hittables
{
    public sealed class YzRect : Hittable
    {
        readonly float _y0;
        readonly float _z0;
        readonly float _y1;
        readonly float _z1;
        readonly float _k;
        readonly Material _material;

        readonly Aabb _aabb;

        public YzRect(float y0, float z0, float y1, float z1, float k, Material materal)
        {
            _y0 = y0;
            _z0 = z0;
            _y1 = y1;
            _z1 = z1;
            _k = k;
            _material = materal;

            // The bounding box must have non-zero width in each dimension, so pad the X
            // dimension a small amount.
            _aabb = new Aabb(new Vec3(k - 0.0001F, y0, z0), new Vec3(k + 0.0001F, y1, z1));
        }

        public override Aabb BoundingBox(float exposureTime) => _aabb;

        public override HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var t = (_k - ray.Origin.X) / ray.Direction.X;
            if (t < tMin || t > tMax)
                return null;

            var y = ray.Origin.Y + t * ray.Direction.Y;
            var z = ray.Origin.Z + t * ray.Direction.Z;
            if (y < _y0 || y > _y1 || z < _z0 || z > _z1)
                return null;

            var p = ray.PositionAt(t);
            var outwardNormal = new Vec3(1, 0, 0);
            var ff = ray.Direction.Dot(outwardNormal) < 0;
            var n = ff ? outwardNormal : -outwardNormal;
            var u = (y - _y0) / (_y1 - _y0);
            var v = (z - _z0) / (_z1 - _z0);
            return new HitRecord(t, p, n, _material, ff, u, v);
        }
    }
}
