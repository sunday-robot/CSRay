using System;

namespace CsRay
{
    /// <summary>RayとHitableの衝突情報</summary>
    public sealed class HitRecord
    {
        /// <summary>衝突点</summary>
        public Vec3 Position { get; private set; }

        /// <summary>衝突点の法線</summary>
        public Vec3 Normal { get; private set; }

        /// <summary>衝突点の表面素材</summary>
        public Material Material { get; }

        /// <summary>レイ軸上の位置</summary>
        public double T { get; }

        public double U { get; }

        public double V { get; }

        public bool FrontFace { get; private set; }

        public HitRecord(double t, Vec3 position, Vec3 normal, Material material)
        {
            T = t;
            Position = position;
            Normal = normal;
            Material = material;
        }

        public void SetFaceNormal(Ray r, Vec3 outwardNormal)
        {
            FrontFace = r.Direction.Dot(outwardNormal) < 0;
            Normal = FrontFace ? outwardNormal : -outwardNormal;
        }

        public void ShiftPosition(Vec3 p) => Position += p;

        internal void SetPosition(Vec3 value)
        {
            Position = value;
        }
    }
}