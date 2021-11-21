namespace CsRay.Materials
{
    /// <summary>
    /// 金属マテリアル
    /// </summary>
    public sealed class Metal : Material
    {
        /// <summary>色</summary>
        readonly Rgb _albedo;

        /// <summary>
        /// <=1
        /// </summary>
        readonly double _fuzz;

        public Metal(Rgb albedo, double fuzz)
        {
            _albedo = albedo;
            _fuzz = fuzz;
        }

        public override (Rgb, Ray)? Scatter(Ray ray, HitRecord rec)
        {
            var reflectionDirection = Reflect(ray.Direction.Unit, rec.Normal);
            var scatteredRay = new Ray(rec.Position, reflectionDirection + _fuzz * Util.RandomInUnitSphere(), 0.0);
            if (scatteredRay.Direction.Dot(rec.Normal) <= 0)
                return null;
            return (_albedo, scatteredRay);
        }
    }
}
