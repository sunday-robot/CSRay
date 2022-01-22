﻿namespace CsRay.Hittables
{
    public sealed class Sphere : Hittable
    {
        /// <summary>位置</summary>
        readonly Vec3 _center;

        /// <summary>半径</summary>
        readonly float _radius;

        readonly Material _material;

        public Sphere(Vec3 center, float radius, Material material)
        {
            _center = center;
            _radius = radius;
            _material = material;
        }

        public override HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var oc = ray.Origin - _center;
            var a = ray.Direction.SquaredLength;
            var halfB = oc.Dot(ray.Direction);
            var c = oc.SquaredLength - _radius * _radius;

            var discriminant = halfB * halfB - a * c;
            if (discriminant < 0)
                return null;

            var d2 = MathF.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            var root = (-halfB - d2) / a;
            if (root < tMin || tMax < root)
            {
                root = (-halfB + d2) / a;
                if (root < tMin || tMax < root)
                    return null;
            }

            var p = ray.PositionAt(root);
            var outwardNormal = (p - _center) / _radius;
            var (u, v) = GetSphereUv(outwardNormal);
            var ff = ray.Direction.Dot(outwardNormal) < 0;
            var n = ff ? outwardNormal : -outwardNormal;
            return new HitRecord(root, p, n, _material, ff, u, v);
        }

        public override Aabb BoundingBox(float exposureTime)
        {
            var min = _center - new Vec3(_radius, _radius, _radius);
            var max = _center + new Vec3(_radius, _radius, _radius);
            return new Aabb(min, max);
        }

        static (float, float) GetSphereUv(Vec3 p)
        {
            // p: a given point on the sphere of radius one, centered at the origin.
            // u: returned value [0,1] of angle around the Y axis from X=-1.
            // v: returned value [0,1] of angle from Y=-1 to Y=+1.
            //     <1 0 0> yields <0.50 0.50>       <-1  0  0> yields <0.00 0.50>
            //     <0 1 0> yields <0.50 1.00>       < 0 -1  0> yields <0.50 0.00>
            //     <0 0 1> yields <0.25 0.50>       < 0  0 -1> yields <0.75 0.50>
            var theta = MathF.Acos(-p.Y);
            var phi = MathF.Atan2(-p.Z, p.X) + MathF.PI;
            var u = phi / (2 * MathF.PI);
            var v = theta / MathF.PI;
            return (u, v);
        }

        public override string ToString()
        {
            return $"Sphere(c:{_center}, r:{_radius}, m:{_material})";
        }
    }
}
