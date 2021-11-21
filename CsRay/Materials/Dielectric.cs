using System;

namespace CsRay.Materials
{
    /// <summary>
    /// 誘電体マテリアル(透明な材質)
    /// </summary>
    public sealed class Dielectric : Material
    {
        /// <summary>屈折率</summary>
        readonly double _refractiveIndex;

        public Dielectric(double refractiveIndex)
        {
            _refractiveIndex = refractiveIndex;
        }

        public override (Rgb, Ray)? Scatter(Ray ray, HitRecord rec)
        {
            var refractionRatio = rec.FrontFace ? (1.0 / _refractiveIndex) : _refractiveIndex;

            var unitDirection = ray.Direction.Unit;
            double cosTheta = Math.Min(-unitDirection.Dot(rec.Normal), 1);
            double sinTheta = Math.Sqrt(1 - cosTheta * cosTheta);

            var cannotRefract = refractionRatio * sinTheta > 1;
            Vec3 direction;

            if (cannotRefract || Reflectance(cosTheta, refractionRatio) > Util.Rand())
                direction = Reflect(unitDirection, rec.Normal);
            else
                direction = Refract(unitDirection, rec.Normal, refractionRatio);

            var scattered = new Ray(rec.Position, direction, ray.Time);
            return (new Rgb(1, 1, 1), scattered);

#if false

            Vec3 outwardNormal;
            double rri; // 相対屈折率(元の素材の屈折率/先の素材の屈折率)
            if (ray.Direction.Dot(rec.Normal) > 0)
            {
                // レイがこの素材の物体から出る場合
                outwardNormal = -rec.Normal;
                rri = _refractiveIndex;
            }
            else
            {
                // レイがこの素材の物体に入る場合
                outwardNormal = rec.Normal;
                rri = 1.0 / _refractiveIndex;
            }
            var refractedDirection = Refract(ray.Direction, outwardNormal, rri);
            if (refractedDirection == null)
                return (attenuation, new Ray(rec.Position, Reflect(ray.Direction, rec.Normal), 0.0));

            var cosine = -ray.Direction.Dot(outwardNormal) / ray.Direction.Length;
            var reflectProb = Util.Schlick(cosine, _refractiveIndex);
            var direction = (Util.Rand() < reflectProb) ? Reflect(ray.Direction, rec.Normal) :
                refractedDirection;
            return (attenuation, new Ray(rec.Position, direction, 0.0));
#endif
        }
        static double Reflectance(double cosine, double refIdx)
        {
            // Use Schlick's approximation for reflectance.
            var r0 = (1 - refIdx) / (1 + refIdx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
        }
    }
}
