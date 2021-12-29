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
        public static readonly Rgb Black = new Rgb(0, 0, 0);

        public double R { get; }

        public double G { get; }

        public double B { get; }

        public Rgb(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static Rgb operator +(Rgb a, Rgb b) => new Rgb(a.R + b.R, a.G + b.G, a.B + b.B);

        public static Rgb operator *(Rgb a, Rgb b) => new Rgb(a.R * b.R, a.G * b.G, a.B * b.B);

        public static Rgb operator *(Rgb a, double b) => new Rgb(a.R * b, a.G * b, a.B * b);

        public static Rgb operator *(double a, Rgb b) => b * a;

        public static Rgb operator /(Rgb a, double b) => new Rgb(a.R / b, a.G / b, a.B / b);

        public override string ToString()
        {
            return $"({R:0.000}, {G:0.000}, {B:0.000})";
        }
    }
}
