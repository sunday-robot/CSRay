namespace CsRay
{
    /// <summary>
    /// 色(光)
    /// 
    /// 物理的な光ではなく、単純にR、G、Bの強度を示す3つの実数からなるものでしかなく、様々な波長の光の混成などではない。
    /// このため、波長による屈折率の違いなどを考慮したレンダリングはできない。
    /// (無理にやろうとしても、3種類の波長しかないので、1本の白い光をプリズムに照射しても出ていくのは虹色の台形ではなく、赤、青、緑の3本の光となってしまう。)
    /// 
    /// 書籍ではVec3で代用していたが、3つの浮動小数点数から構成されているということが
    /// たまたま一致しているだけで別物(内積や外積などRGBに定義できない)というだけなので、
    /// 同じクラスにはしていない。
    /// </summary>
    public sealed class Rgb
    {
        public static readonly Rgb Black = new(0, 0, 0);

        public double R { get; }

        public double G { get; }

        public double B { get; }

        public Rgb(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static Rgb operator +(Rgb a, Rgb b) => new(a.R + b.R, a.G + b.G, a.B + b.B);

        public static Rgb operator *(Rgb a, Rgb b) => new(a.R * b.R, a.G * b.G, a.B * b.B);

        public static Rgb operator *(Rgb a, double b) => new(a.R * b, a.G * b, a.B * b);

        public static Rgb operator *(double a, Rgb b) => b * a;

        public static Rgb operator /(Rgb a, double b) => new(a.R / b, a.G / b, a.B / b);

        public override string ToString()
        {
            return $"({R:0.000}, {G:0.000}, {B:0.000})";
        }
    }
}
