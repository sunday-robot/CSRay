using System;

namespace CsRay.Hittables
{
    public sealed class MovingSphere : Hittable
    {
        /// <summary>t=0での位置</summary>
        readonly Vec3 _centerOrigin;

        /// <summary>速度</summary>
        readonly Vec3 _v;

        /// <summary>半径</summary>
        readonly double _radius;

        readonly Material _material;

        public MovingSphere(Vec3 center0, double time0, Vec3 center1, double time1, double radius, Material material)
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

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            var center = Center(ray.Time);
            var oc = ray.Origin - center;
            var a = ray.Direction.SquaredLength;
            var halfB = oc.Dot(ray.Direction);
            var c = oc.SquaredLength - _radius * _radius;
            var discriminant = halfB * halfB - a * c;
            if (discriminant < 0)
                return false;
            var d2 = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            var t = (-halfB - d2) / a;
            if (t < tMin || tMax < t)
            {
                t = (-halfB + d2) / a;
                if (t < tMin || tMax < t)
                    return false;
            }

            var p = ray.PositionAt(t);
            var outwardNormal = (p - center) / _radius;

            rec.SetT(t);
            rec.SetPosition(p);
            rec.SetFaceNormal(ray, outwardNormal);
            rec.SetMaterial(_material);

            return true;
        }

        public override Aabb BoundingBox(double t0, double t1)
        {
            var c0 = Center(t0);
            var c1 = Center(t1);
            var v = new Vec3(_radius, _radius, _radius);
            var box0 = new Aabb(c0 - v, c0 + v);
            var box1 = new Aabb(c1 - v, c1 + v);
            return Aabb.SurroundingAabb(box0, box1);
        }

        public override string ToString()
        {
            return $"MovigSphere(c:{_centerOrigin}, r:{_radius}, m:{_material}, v:{_v})";
        }
    }
}
