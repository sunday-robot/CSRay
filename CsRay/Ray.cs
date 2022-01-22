namespace CsRay
{
    public sealed class Ray
    {
        /// <summary>起点</summary>
        public Vec3 Origin { get; }

        /// <summary>方向</summary>
        public Vec3 Direction { get; }

        public double Time { get; }

        public Ray(Vec3 origin, Vec3 direction, double time)
        {
            Origin = origin;
            Direction = direction;
            Time = time;
        }

        public Vec3 PositionAt(double t) => Origin + Direction * t;
    }
}
