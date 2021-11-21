namespace CsRay
{
    public abstract class Texture
    {
        public abstract Rgb Value(double u, double v, Vec3 p);
    }
}
