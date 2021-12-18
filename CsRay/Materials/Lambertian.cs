using CsRay.Textures;

namespace CsRay.Materials
{
    /// <summary>
    /// 完全な拡散反射をする材質(入射角とは無関係に反射する)
    /// </summary>
    public class Lambertian : Material
    {
        /// <summary>素材の色</summary>
        readonly Texture _albedo;

        public Lambertian(Texture albedo)
        {
            _albedo = albedo;
        }

        public Lambertian(Rgb rgb) : this(new SolidColor(rgb)) { }

        public Lambertian(double r, double g, double b) : this(new Rgb(r, g, b)) { }

        public override Rgb Emitted(double u, double v, Vec3 p) => Rgb.Black;

        public override bool Scatter(Ray ray, ref HitRecord rec, out Rgb attenuation, out Ray scattered)
        {
            var scatterDirection = rec.Normal + Util.RandomInUnitSphere();

            // Catch degenerate scatter direction
            if (scatterDirection.NearZero())
                scatterDirection = rec.Normal;

            scattered = new Ray(rec.Position, scatterDirection, ray.Time);
            attenuation = _albedo.Value(rec.U, rec.V, rec.Position);
            return true;
        }

        public override string ToString()
        {
            return $"Lambertian({_albedo})";
        }
    }
}
