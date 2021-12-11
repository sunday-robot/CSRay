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
#if false
            return HitSub(Min.X, Max.X, ray.Origin.X, ray.Direction.X, ref tMin, ref tMax)
                && HitSub(Min.Y, Max.Y, ray.Origin.X, ray.Direction.Y, ref tMin, ref tMax)
                && HitSub(Min.Z, Max.Z, ray.Origin.Z, ray.Direction.Z, ref tMin, ref tMax);
#else
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
#endif
        }

#if false
        static bool HitSub(double min, double max,
             double rayOrigin, double rayDirection,
             ref double tMin, ref double tMax)
        {
            var a = (min - rayOrigin) / rayDirection;
            var b = (max - rayOrigin) / rayDirection;
            tMin = Math.Max(a, tMin);
            tMax = Math.Min(b, tMax);
            return tMin < tMax;
        }
#endif

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
