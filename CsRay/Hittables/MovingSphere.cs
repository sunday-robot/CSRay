using System;

namespace CsRay.Hitables
{
    public sealed class MovingSphere : Hittable
    {
        /// <summary></summary>
        readonly Vec3 _centerOrigin;
        readonly Vec3 _v;
        readonly double _radius;
        readonly Material _material;

        public MovingSphere(
            Vec3 center0, double time0, Vec3 center1, double time1, double radius, Material material)
        {
            var dt = time1 - time0;
            _centerOrigin = (center0 * time1 - center1 * time0) / dt;
            _v = (center1 - center0) / dt;

            _radius = radius;
            _material = material;
        }

        Vec3 Center(double time)
        {
            return _centerOrigin + _v * time;
        }

        public override HitRecord Hit(Ray ray, double tMin, double tMax, HitRecord _)
        {
            var center = Center(ray.Time);
            var oc = ray.Origin - center;
            var a = ray.Direction.SquaredLength;
            var b = oc.Dot(ray.Direction);
            var c = oc.SquaredLength - _radius * _radius;
            var discriminant = b * b - a * c;
            if (discriminant < 0)
                return null;
            var d2 = Math.Sqrt(discriminant);
            var t = (-b - d2) / a;
            if (t > tMin && t < tMax)
            {
                var p = ray.PositionAt(t);
                var normal = (p - center) / _radius;
                return new HitRecord(t, p, normal, _material);
            }
            var t2 = (-b + d2) / a;
            if (t2 > tMin && t2 < tMax)
            {
                var p = ray.PositionAt(t2);
                var normal = (p - center) / _radius;
                return new HitRecord(t2, p, normal, _material);
            }
            return null;
        }

        public override Aabb BoundingBox(double t0, double t1)
        {
            var c0 = Center(t0);
            var c1 = Center(t1);
            var (minX, maxX) = BoundingBoxSub(c0.X, c1.X);
            var (minY, maxY) = BoundingBoxSub(c0.Y, c1.Y);
            var (minZ, maxZ) = BoundingBoxSub(c0.Z, c1.Z);
            return new Aabb(new Vec3(minX, minY, minZ), new Vec3(maxX, maxY, maxZ));
        }

        (double, double) BoundingBoxSub(double c0, double c1)
        {
            return (c0 < c1) ? (c0 - _radius, c1 + _radius) : (c1 - _radius, c0 + _radius);
        }
    }
}
