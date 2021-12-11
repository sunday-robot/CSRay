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

        public override Rgb Emitted(double u, double v, Vec3 p) => Rgb.Black;

        public override bool Scatter(Ray ray, ref HitRecord rec, out Rgb attenuation, out Ray scattered)
        {
            var reflectionDirection = Reflect(ray.Direction.Unit, rec.Normal);
            scattered = new Ray(rec.Position, reflectionDirection + _fuzz * Util.RandomInUnitSphere(), ray.Time);
            attenuation = _albedo;
            return scattered.Direction.Dot(rec.Normal) > 0;
        }

        public override string ToString()
        {
            return $"Metal({_albedo}, fuzz:{_fuzz:0.000})";
        }
    }
}
