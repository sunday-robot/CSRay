using CsRay.Textures;

namespace CsRay.Materials
{
    public sealed class DiffuseLight : Material
    {
        readonly Texture _emit;

        public DiffuseLight(Texture texture)
        {
            _emit = texture;
        }

        public DiffuseLight(Rgb rgb) : this(new SolidColor(rgb)) { }

        public DiffuseLight(double r, double g, double b) : this(new Rgb(r, g, b)) { }

        public override bool Scatter(Ray ray, ref HitRecord rec, out Rgb attenuation, out Ray scattered)
        {
            attenuation = null;
            scattered = null;
            return false;
        }

        public override Rgb Emitted(double u, double v, Vec3 p) => _emit.Value(u, v, p);
    }
}
