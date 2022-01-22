namespace CsRay.Materials
{
    public sealed class DebugMaterial : Material
    {
        readonly Rgb _color = Util.RandomSaturatedRgb(1, 1);

        public override Rgb Emitted(float u, float v, Vec3 p) => _color;

        public override (Rgb, Ray)? Scatter(Ray ray, HitRecord rec) => null;
    }
}
