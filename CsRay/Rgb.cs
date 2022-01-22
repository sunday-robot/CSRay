namespace CsRay
{
    /// <summary>
    /// 色(光)
    /// 
    /// 実際の光は、様々な波長の混成であるので、物理的には全く不正確なものである。
    /// 波長による屈折率の違いなどがないので、虹はできない。
    /// 
    /// Vec3とは3つの浮動小数点数から構成されているということがたまたま一致しているというだけなので、同じクラスにはしていない。
    /// </summary>
    public sealed class Rgb
    {
        public static readonly Rgb Black = new(0, 0, 0);

        public float R { get; }

        public float G { get; }

        public float B { get; }

        public Rgb(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static Rgb operator +(Rgb a, Rgb b) => new(a.R + b.R, a.G + b.G, a.B + b.B);

        public static Rgb operator *(Rgb a, Rgb b) => new(a.R * b.R, a.G * b.G, a.B * b.B);

        public static Rgb operator *(Rgb a, float b) => new(a.R * b, a.G * b, a.B * b);

        public static Rgb operator *(float a, Rgb b) => b * a;

        public static Rgb operator /(Rgb a, float b) => new(a.R / b, a.G / b, a.B / b);

        public override string ToString()
        {
            return $"({R:0.000}, {G:0.000}, {B:0.000})";
        }
    }
}
