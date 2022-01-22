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

        public override Rgb Emitted(double u, double v, Vec3 p) => Rgb.Black;

        public override (Rgb, Ray)? Scatter(Ray ray, HitRecord rec)
        {
            var attenuation = new Rgb(1, 1, 1);

            var refractionRatio = rec.FrontFace ? (1 / _refractiveIndex) : _refractiveIndex;

            var unitDirection = ray.Direction.Unit;
            var dt = unitDirection.Dot(rec.Normal);
            var cosTheta = Math.Min(-dt, 1);
            var sinTheta = Math.Sqrt(1 - cosTheta * cosTheta);

            //var cannotRefract = refractionRatio * sinTheta > 1;
            Vec3 direction;
            if (refractionRatio * sinTheta > 1)
            {
                direction = Reflect(unitDirection, rec.Normal);
            }
            else
            {
                if (Reflectance(cosTheta, refractionRatio) > Util.Rand())
                    direction = Reflect(unitDirection, rec.Normal);
                else
                {
#if false
                direction = Refract(unitDirection, rec.Normal, refractionRatio);
#else
                    var discriminant = 1 - refractionRatio * refractionRatio * (1 - dt * dt);
                    if (discriminant <= 0)
                    {
                        return null;
                    }
                    direction = refractionRatio * (unitDirection - rec.Normal * dt)
                        - rec.Normal * Math.Sqrt(discriminant);
#endif
                }
            }
            var scattered = new Ray(rec.Position, direction, ray.Time);

            return (attenuation, scattered);
        }

        static double Reflectance(double cosine, double refIdx)
        {
            // Use Schlick's approximation for reflectance.
            var r0 = (1 - refIdx) / (1 + refIdx);
            var r02 = r0 * r0;
            return r0 + (1 - r02) * Math.Pow((1 - cosine), 5);
        }

        public override string ToString()
        {
            return $"Dielectric({_refractiveIndex})";
        }
    }
}
