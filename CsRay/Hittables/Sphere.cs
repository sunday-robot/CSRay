using System;

namespace CsRay.Hittables
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

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            var oc = ray.Origin - _center;
            var a = ray.Direction.SquaredLength;
            var halfB = oc.Dot(ray.Direction);
            var c = oc.SquaredLength - _radius * _radius;

            var discriminant = halfB * halfB - a * c;
            if (discriminant < 0)
                return false;

            var d2 = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            var root = (-halfB - d2) / a;
            if (root < tMin || tMax < root)
            {
                root = (-halfB + d2) / a;
                if (root < tMin || tMax < root)
                    return false;
            }

            rec.SetT(root);
            rec.SetPosition(ray.PositionAt(rec.T));
            var outwardNormal = (rec.Position - _center) / _radius;
            rec.SetFaceNormal(ray, outwardNormal);
            var (u, v) = GetSphereUv(outwardNormal);
            rec.SetUv(u, v);
            rec.SetMaterial(_material);

            return true;
        }

        public override Aabb BoundingBox(double t0, double t1)
        {
            var min = _center - new Vec3(_radius, _radius, _radius);
            var max = _center + new Vec3(_radius, _radius, _radius);
            return new Aabb(min, max);
        }


        static (double, double) GetSphereUv(Vec3 p)
        {
            // p: a given point on the sphere of radius one, centered at the origin.
            // u: returned value [0,1] of angle around the Y axis from X=-1.
            // v: returned value [0,1] of angle from Y=-1 to Y=+1.
            //     <1 0 0> yields <0.50 0.50>       <-1  0  0> yields <0.00 0.50>
            //     <0 1 0> yields <0.50 1.00>       < 0 -1  0> yields <0.50 0.00>
            //     <0 0 1> yields <0.25 0.50>       < 0  0 -1> yields <0.75 0.50>
            var theta = Math.Acos(-p.Y);
            var phi = Math.Atan2(-p.Z, p.X) + Math.PI;
            var u = phi / (2 * Math.PI);
            var v = theta / Math.PI;
            return (u, v);
        }
        public override string ToString()
        {
            return $"Sphere(c:{_center}, r:{_radius}, m:{_material})";
        }
    }
}
