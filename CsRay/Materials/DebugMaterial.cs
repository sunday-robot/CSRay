namespace CsRay.Materials
{
    public sealed class DebugMaterial : Material
    {
        readonly Rgb _color = Util.RandomSaturatedRgb(1, 1);

        public override Rgb Emitted(double u, double v, Vec3 p)
        {
            return _color;
        }

        public override bool Scatter(Ray ray, ref HitRecord rec, out Rgb attenuation, out Ray scattered)
        {
            attenuation = null;
            scattered = null;
            return false;
        }
    }
}
