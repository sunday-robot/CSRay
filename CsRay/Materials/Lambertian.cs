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

        public Lambertian(float r, float g, float b) : this(new Rgb(r, g, b)) { }

        public override Rgb Emitted(float u, float v, Vec3 p) => Rgb.Black;

        public override (Rgb, Ray)? Scatter(Ray ray, HitRecord rec)
        {
            var scatterDirection = rec.Normal + Util.RandomInUnitSphere();

            // Catch degenerate scatter direction
            if (scatterDirection.NearZero())
                scatterDirection = rec.Normal;

            var scattered = new Ray(rec.Position, scatterDirection, ray.Time);
            var attenuation = _albedo.Value(rec.U, rec.V, rec.Position);
            return (attenuation, scattered);
        }

        public override string ToString()
        {
            return $"Lambertian({_albedo})";
        }
    }
}
