namespace CsRay.Hittables
{
    public sealed class XyRect : Hittable
    {
        readonly float _x0;
        readonly float _y0;
        readonly float _x1;
        readonly float _y1;
        readonly float _k;
        readonly Material _material;

        readonly Aabb _aabb;

        public XyRect(float x0, float y0, float x1, float y1, float k, Material material)
        {
            _x0 = x0;
            _y0 = y0;
            _x1 = x1;
            _y1 = y1;
            _k = k;
            _material = material;

            // The bounding box must have non-zero width in each dimension, so pad the Z
            // dimension a small amount.
            _aabb = new Aabb(new Vec3(x0, y0, k - 0.0001F), new Vec3(x1, y1, k + 0.0001F));
        }

        public override Aabb BoundingBox(float exposureTime) => _aabb;

        public override HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var t = (_k - ray.Origin.Z) / ray.Direction.Z;
            if (t < tMin || t > tMax)
                return null;

            var x = ray.Origin.X + t * ray.Direction.X;
            var y = ray.Origin.Y + t * ray.Direction.Y;
            if (x < _x0 || x > _x1 || y < _y0 || y > _y1)
                return null;

            var p = ray.PositionAt(t);
            var outwardNormal = new Vec3(0, 0, 1);
            var ff = ray.Direction.Dot(outwardNormal) < 0;
            var n = ff ? outwardNormal : -outwardNormal;
            var u = (x - _x0) / (_x1 - _x0);
            var v = (y - _y0) / (_y1 - _y0);
            return new HitRecord(t, p, n, _material, ff, u, v);
        }
    }
}
