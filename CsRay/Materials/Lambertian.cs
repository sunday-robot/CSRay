namespace CsRay.Materials
{
    /// <summary>
    /// 完全な拡散反射をする材質(入射角とは無関係に反射する)
    /// </summary>
    public class Lambertian : Material
    {
        /// <summary>素材の色</summary>
        readonly Texture _albedo;

        public Lambertian(Texture albedo)
        {
            _albedo = albedo;
        }

        public Lambertian(Rgb rgb) : this(new SolidColor(rgb)) { }

        public override (Rgb, Ray)? Scatter(Ray ray, HitRecord rec)
        {
            var direction = rec.Normal + Util.RandomInUnitSphere();

            // Catch degenerate scatter direction
            if (direction.NearZero())
                direction = rec.Normal;

            var scatteredRay = new Ray(rec.Position, direction, ray.Time);
            return (_albedo.Value(rec.U, rec.V, rec.Position), scatteredRay);
        }
    }
}
