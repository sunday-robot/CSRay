using System;

namespace CsRay
{
    internal class CheckerTexture : Texture
    {
        readonly Texture _even;
        readonly Texture _odd;

        public CheckerTexture(Texture even, Texture odd)
        {
            _even = even;
            _odd = odd;
        }

        public CheckerTexture(Rgb even, Rgb odd) : this(new SolidColor(even), new SolidColor(odd)) { }

        public override Rgb Value(double u, double v, Vec3 p)
        {
            var sines = Math.Sin(10 * p.X) * Math.Sin(10 * p.Y) * Math.Sin(10 * p.Z);
            if (sines < 0)
                return _odd.Value(u, v, p);
            else
                return _even.Value(u, v, p);
        }

        public override string ToString()
        {
            return $"CheckerTexture({_even}, {_odd})";
        }
    }
}
