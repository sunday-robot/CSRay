namespace CsRay.Textures
{
    public sealed class SolidColor : Texture
    {
        readonly Rgb _value;

        public SolidColor(Rgb c)
        {
            _value = c;
        }

        public SolidColor(float red, float green, float blue) : this(new Rgb(red, green, blue)) { }

        public override Rgb Value(float u, float v, Vec3 p) => _value;

        public override string ToString()
        {
            return $"SolidColor({_value})";
        }
    }
}
