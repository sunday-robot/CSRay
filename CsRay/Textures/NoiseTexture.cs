namespace CsRay.Textures
{
    public sealed class NoiseTexture : Texture
    {
        readonly Perlin _noise = new();
        readonly float _scale;

        public NoiseTexture(float scale)
        {
            _scale = scale;
        }

        public override Rgb Value(float u, float v, Vec3 p)
        {
            // return color(1,1,1)*0.5*(1 + noise.turb(scale * p));
            // return color(1,1,1)*noise.turb(scale * p);
            return (new Rgb(1, 1, 1)) * 0.5F * (1 + MathF.Sin(_scale * p.Z + 10 * _noise.Turb(p)));
        }
    }
}
