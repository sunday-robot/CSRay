namespace CsRay.Hittables
{
    public sealed class MovingSphere : Hittable
    {
        /// <summary>位置</summary>
        readonly Vec3 _center;

        /// <summary>半径</summary>
        readonly double _radius;

        readonly Material _material;

        /// <summary>速度</summary>
        readonly Vec3 _velocity;

        public MovingSphere(Vec3 center, double radius, Material material, Vec3 velocity)
        {
            _center = center;
            _radius = radius;
            _material = material;
            _velocity = velocity;
        }

        public override HitRecord? Hit(Ray ray, double tMin, double tMax)
        {
            var center = _center + _velocity * ray.Time;
            var oc = ray.Origin - center;
            var a = ray.Direction.SquaredLength;
            var halfB = oc.Dot(ray.Direction);
            var c = oc.SquaredLength - _radius * _radius;

            var discriminant = halfB * halfB - a * c;
            if (discriminant < 0)
                return null;

            var d2 = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            var root = (-halfB - d2) / a;
            if (root < tMin || tMax < root)
            {
                root = (-halfB + d2) / a;
                if (root < tMin || tMax < root)
                    return null;
            }

            var p = ray.PositionAt(root);
            var outwardNormal = (p - center) / _radius;
            var (u, v) = GetSphereUv(outwardNormal);
            var ff = ray.Direction.Dot(outwardNormal) < 0;
            var n = ff ? outwardNormal : -outwardNormal;
            return new HitRecord(root, p, n, _material, ff, u, v);
        }

        public override Aabb BoundingBox(double dt)
        {
            var c0 = _center - _velocity * dt / 2;
            var c1 = _center + _velocity * dt / 2;
            var v = new Vec3(_radius, _radius, _radius);
            var box0 = new Aabb(c0 - v, c0 + v);
            var box1 = new Aabb(c1 - v, c1 + v);
            return Aabb.SurroundingAabb(box0, box1);
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
            return $"MovigSphere(c:{_center}, r:{_radius}, m:{_material}, v:{_velocity})";
        }
    }
}
