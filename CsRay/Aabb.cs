using System;

namespace CsRay
{
    /// <summary>Axis Aligned Bounding Box</summary>
    public sealed class Aabb
    {
        public Vec3 Min { get; }

        public Vec3 Max { get; }

        public Aabb(Vec3 min, Vec3 max)
        {
            Min = min;
            Max = max;
        }

        public bool Hit(Ray ray, double tMin, double tMax)
        {
            double t;

            t = Math.Min((Min.X - ray.Origin.X) / ray.Direction.X,
                           (Max.X - ray.Origin.X) / ray.Direction.X);
            tMin = Math.Max(t, tMin);
            t = Math.Max((Min.X - ray.Origin.X) / ray.Direction.X,
                           (Max.X - ray.Origin.X) / ray.Direction.X);
            tMax = Math.Min(t, tMax);
            if (tMax <= tMin)
                return false;

            t = Math.Min((Min.Y - ray.Origin.Y) / ray.Direction.Y,
                           (Max.Y - ray.Origin.Y) / ray.Direction.Y);
            tMin = Math.Max(t, tMin);
            t = Math.Max((Min.Y - ray.Origin.Y) / ray.Direction.Y,
                           (Max.Y - ray.Origin.Y) / ray.Direction.Y);
            tMax = Math.Min(t, tMax);
            if (tMax <= tMin)
                return false;

            t = Math.Min((Min.Z - ray.Origin.Z) / ray.Direction.Z,
                           (Max.Z - ray.Origin.Z) / ray.Direction.Z);
            tMin = Math.Max(t, tMin);
            t = Math.Max((Min.Z - ray.Origin.Z) / ray.Direction.Z,
                           (Max.Z - ray.Origin.Z) / ray.Direction.Z);
            tMax = Math.Min(t, tMax);
            if (tMax <= tMin)
                return false;
            return true;
        }

        public static Aabb SurroundingAabb(Aabb a, Aabb b)
        {
            var minX = Math.Min(a.Min.X, b.Min.X);
            var maxX = Math.Max(a.Max.X, b.Max.X);
            var minY = Math.Min(a.Min.Y, b.Min.Y);
            var maxY = Math.Max(a.Max.Y, b.Max.Y);
            var minZ = Math.Min(a.Min.Z, b.Min.Z);
            var maxZ = Math.Max(a.Max.Z, b.Max.Z);
            return new Aabb(new Vec3(minX, minY, minZ), new Vec3(maxX, maxY, maxZ));
        }

        public override string ToString()
        {
            return $"{Min}-{Max}";
        }
    }
}
