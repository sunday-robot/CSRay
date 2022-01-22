using CsRay.Textures;

namespace CsRay.Materials
{
    public sealed class Isotropic : Material
    {
        readonly Texture _albedo;

        public Isotropic(Texture texture)
        {
            _albedo = texture;
        }

        public Isotropic(Rgb rgb) : this(new SolidColor(rgb)) { }

        public override Rgb Emitted(float u, float v, Vec3 p) => Rgb.Black;

        public override (Rgb, Ray)? Scatter(Ray ray, HitRecord rec)
        {
            var attenuation = _albedo.Value(rec.U, rec.V, rec.Position);
            var scattered = new Ray(rec.Position, Util.RandomInUnitSphere(), ray.Time);
            return (attenuation, scattered);
        }
    }
}
