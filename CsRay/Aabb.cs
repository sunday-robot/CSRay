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
            var (xMin, xMax) = HitSub(Min.X, Max.X, ray.Origin.X, ray.Direction.X, tMin, tMax);
            if (xMax <= xMin)
                return false;
            var (yMin, yMax) = HitSub(Min.Y, Max.Y, ray.Origin.X, ray.Direction.Y, xMin, xMax);
            if (yMax <= yMin)
                return false;
            var (zMin, zMax) = HitSub(Min.Z, Max.Z, ray.Origin.Z, ray.Direction.Z, yMin, yMax);
            if (zMax <= zMin)
                return false;
            return true;
        }

        static (double, double) HitSub(double min, double max,
             double rayOrigin, double rayDirection,
             double tMin, double tMax)
        {
            var a = (min - rayOrigin) / rayDirection;
            var b = (max - rayOrigin) / rayDirection;
            if (a > b)
                (a, b) = (b, a);
            return (Math.Max(a, tMin), Math.Min(b, tMax));
        }

        public static Aabb SurroundingAabb(Aabb a, Aabb b)
        {
            var minX = Math.Min(a.Min.X, b.Min.X);
            var maxX = Math.Min(a.Max.X, b.Max.X);
            var minY = Math.Min(a.Min.Y, b.Min.Y);
            var maxY = Math.Max(a.Max.Y, b.Max.Y);
            var minZ = Math.Max(a.Min.Z, b.Min.Z);
            var maxZ = Math.Max(a.Max.Z, b.Max.Z);
            return new Aabb(new Vec3(minX, minY, minZ), new Vec3(maxX, maxY, maxZ));
        }
    }
}
