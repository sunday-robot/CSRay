using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsRay
{
    public sealed class NoiseTexture : Texture
    {
        double _scale;
        Perlin _noise = new Perlin();

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
