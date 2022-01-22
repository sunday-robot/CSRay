using CsRay.Materials;
using CsRay.Textures;

namespace CsRay.Hittables
{
    public sealed class ConstantMedium : Hittable
    {
        static readonly Vec3 _dummyNormal = new(0, 0, 0);
        readonly Hittable _boundary;
        readonly Material _phaseFunction;
        readonly float _negInvDensity;

        public ConstantMedium(Hittable b, float d, Texture a)
        {
            _boundary = b;
            _negInvDensity = -1 / d;
            _phaseFunction = new Isotropic(a);
        }

        public ConstantMedium(Hittable b, float d, Rgb c) : this(b, d, new SolidColor(c)) { }

        public override HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            float t1;
            float t2;
            {
                // レイの後方も含めて交点があるかチェックする。
                var rec1 = _boundary.Hit(ray, float.NegativeInfinity, float.PositiveInfinity);
                if (rec1 == null)
                    return null;

                // レイの前方に交点があるかチェックする
                var rec2 = _boundary.Hit(ray, rec1.T + 0.0001F, float.PositiveInfinity);
                if (rec2 == null)
                    return null;

                t1 = Math.Max(rec1.T, tMin);
                t2 = Math.Min(rec2.T, tMax);
            }

            if (t1 >= t2)
                return null;

            var rayLength = ray.Direction.Length;
            var distanceInsideBoundary = (t2 - t1) * rayLength;
            var hitDistance = _negInvDensity * MathF.Log(Util.Rand());

            if (hitDistance > distanceInsideBoundary)
                return null;

            var t = t1 + hitDistance / rayLength;
            var p = ray.PositionAt(t);
            return new HitRecord(t, p, _dummyNormal, _phaseFunction);
        }

        public override Aabb BoundingBox(float exposureTime) => _boundary.BoundingBox(exposureTime);
    }
}