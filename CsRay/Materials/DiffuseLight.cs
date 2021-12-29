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

        public override (Rgb, Ray)? Scatter(Ray ray, HitRecord rec) => null;

        public override Rgb Emitted(double u, double v, Vec3 p) => _emit.Value(u, v, p);
    }
}
