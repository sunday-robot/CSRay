namespace CsRay
{
    public abstract class Texture
    {
        public abstract Rgb Value(float u, float v, Vec3 p);
    }
}
