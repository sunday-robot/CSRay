namespace CsRay
{
    public sealed class Vec3
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Length => MathF.Sqrt(SquaredLength);

        public float SquaredLength => X * X + Y * Y + Z * Z;

        /// <summary>長さを1にしたベクトル</summary>
        public Vec3 Unit => this / Length;

        public static Vec3 operator -(Vec3 a) => new(-a.X, -a.Y, -a.Z);

        public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vec3 operator *(Vec3 a, float b) => new(a.X * b, a.Y * b, a.Z * b);

        public static Vec3 operator *(float a, Vec3 b) => b * a;

        public static Vec3 operator /(Vec3 a, float b) => new(a.X / b, a.Y / b, a.Z / b);

        /// <returns>内積</returns>
        public float Dot(Vec3 a) => X * a.X + Y * a.Y + Z * a.Z;

        /// <returns>外積</returns>
        public Vec3 Cross(Vec3 a) => new(Y * a.Z - Z * a.Y, Z * a.X - X * a.Z, X * a.Y - Y * a.X);

        public override string ToString()
        {
            return $"({X:0.000}, {Y:0.000}, {Z:0.000})";
        }

        public bool NearZero()
        {
            // Return true if the vector is close to zero in all dimensions.
            var s = 1e-8F;
            return (Math.Abs(X) < s) && (Math.Abs(Y) < s) && (Math.Abs(Z) < s);
        }
    }
}
