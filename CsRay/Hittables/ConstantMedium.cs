using CsRay.Materials;
using CsRay.Textures;
using System;

namespace CsRay.Hittables
{
    public sealed class ConstantMedium : Hittable
    {
        readonly Hittable _boundary;
        readonly Material _phaseFunction;
        readonly double _negInvDensity;

        public ConstantMedium(Hittable b, double d, Texture a)
        {
            _boundary = b;
            _negInvDensity = -1 / d;
            _phaseFunction = new Isotropic(a);
        }

        public ConstantMedium(Hittable b, double d, Rgb c) : this(b, d, new SolidColor(c)) { }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            bool enableDebug = false;
            bool debugging = enableDebug && Util.Rand() < 0.00001;

            var rec1 = new HitRecord(0, new Vec3(0, 0, 0), new Vec3(0, 0, 0), null);
            var rec2 = new HitRecord(0, new Vec3(0, 0, 0), new Vec3(0, 0, 0), null);

            if (!_boundary.Hit(ray, double.NegativeInfinity, double.PositiveInfinity, ref rec1))
                return false;

            if (!_boundary.Hit(ray, rec1.T + 0.0001, double.PositiveInfinity, ref rec2))
                return false;

            if (debugging) Console.WriteLine($"\nt_min={rec1.T}, tMax={rec2.T}");

            if (rec1.T < tMin)
                rec1.SetT(tMin);
            if (rec2.T > tMax)
                rec2.SetT(tMax);

            if (rec1.T >= rec2.T)
                return false;

            if (rec1.T < 0)
                rec1.SetT(0);

            var rayLength = ray.Direction.Length;
            var distanceInsideBoundary = (rec2.T - rec1.T) * rayLength;
            var hitDistance = _negInvDensity * Math.Log(Util.Rand());

            if (hitDistance > distanceInsideBoundary)
                return false;

            rec.SetT(rec1.T + hitDistance / rayLength);
            rec.SetPosition(ray.PositionAt(rec.T));

            if (debugging)
            {
                Console.WriteLine($"hit_distance = {hitDistance}");
                Console.WriteLine($"rec.T = {rec.T}");
                Console.WriteLine($"rec.p = {rec.Position}");
            }

            rec.SetNormal(new Vec3(1, 0, 0));  // arbitrary
            rec.SetFrontFace(true);     // also arbitrary
            rec.SetMaterial(_phaseFunction);

            return true;
        }

        public override Aabb BoundingBox(double t0, double t1) => _boundary.BoundingBox(t0, t1);
    }
}