namespace CsRay
{
    public sealed class Ray
    {
        /// <summary>起点</summary>
        public Vec3 Origin { get; }

        /// <summary>方向</summary>
        public Vec3 Direction { get; }

        public float Time { get; }

        public Ray(Vec3 origin, Vec3 direction, float time)
        {
            Origin = origin;
            Direction = direction;
            Time = time;
        }

        public Vec3 PositionAt(float t) => Origin + Direction * t;
    }
}
