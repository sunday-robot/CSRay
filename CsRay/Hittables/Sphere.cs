using System;

namespace CsRay.Hitables
{
    public sealed class Sphere : Hittable
    {
        readonly Vec3 _center;
        readonly double _radius;
        readonly Material _material;

        public Sphere(Vec3 center, double radius, Material material)
        {
            _center = center;
            _radius = radius;
            _material = material;
        }

        public override HitRecord Hit(Ray ray, double tMin, double tMax, HitRecord _)
        {
            var oc = ray.Origin - _center;
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
                var normal = (p - _center) / _radius;
                return new HitRecord(t, p, normal, _material);
            }
            var t2 = (-b + d2) / a;
            if (t2 > tMin && t2 < tMax)
            {
                var p = ray.PositionAt(t2);
                var normal = (p - _center) / _radius;
                return new HitRecord(t2, p, normal, _material);
            }
            return null;
        }

        public override Aabb BoundingBox(double t0, double t1)
        {
            var min = _center - new Vec3(_radius, _radius, _radius);
            var max = _center + new Vec3(_radius, _radius, _radius);
            return new Aabb(min, max);
        }
    }
}
