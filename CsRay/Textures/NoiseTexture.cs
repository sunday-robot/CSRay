using System;

namespace CsRay.Textures
{
    public sealed class NoiseTexture : Texture
    {
        readonly Perlin _noise = new Perlin();
        readonly double _scale;

        public NoiseTexture(double scale)
        {
            _scale = scale;
        }

        public override Rgb Value(double u, double v, Vec3 p)
        {
            // return color(1,1,1)*0.5*(1 + noise.turb(scale * p));
            // return color(1,1,1)*noise.turb(scale * p);
            return (new Rgb(1, 1, 1)) * 0.5 * (1 + Math.Sin(_scale * p.Z + 10 * _noise.Turb(p)));
        }

    }
}
