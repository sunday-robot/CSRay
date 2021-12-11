namespace CsRay.Materials
{
    public sealed class Isotropic : Material
    {
        readonly Texture _albedo;

        public Isotropic(Rgb rgb) : this(new SolidColor(rgb)) { }

        public Isotropic(Texture texture)
        {
            _albedo = texture;
        }

        public override Rgb Emitted(double u, double v, Vec3 p) => Rgb.Black;

        public override bool Scatter(Ray ray, ref HitRecord rec, out Rgb attenuation, out Ray scattered)
        {
            attenuation = _albedo.Value(rec.U, rec.V, rec.Position);
            scattered = new Ray(rec.Position, Util.RandomInUnitSphere(), ray.Time);
            return true;
        }
    }
}
