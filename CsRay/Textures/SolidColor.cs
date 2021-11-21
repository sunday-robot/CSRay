namespace CsRay
{
    public sealed class SolidColor : Texture
    {
        readonly Rgb _value;
        public SolidColor(Rgb c)
        {
            _value = c;
        }
        public SolidColor(double red, double green, double blue) : this(new Rgb(red, green, blue)) { }

        public override Rgb Value(double u, double v, Vec3 p)
        {
            return _value;
        }
    }
}
