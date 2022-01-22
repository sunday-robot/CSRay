namespace CsRay
{
    /// <summary>RayとHittableの衝突情報</summary>
    public sealed class HitRecord
    {
        /// <summary>衝突点</summary>
        public Vec3 Position { get; }

        /// <summary>衝突点の法線</summary>
        public Vec3 Normal { get; }

        /// <summary>衝突点の表面素材</summary>
        public Material Material { get; }

        /// <summary>レイ軸上の位置</summary>
        public float T { get; }

        public float U { get; }

        public float V { get; }

        public bool FrontFace { get; }

        public HitRecord(float t, Vec3 position, Vec3 normal, Material material, bool frontFace = false, float u = 0, float v = 0)
        {
            T = t;
            Position = position;
            Normal = normal;
            Material = material;
            FrontFace = frontFace;
            U = u;
            V = v;
        }
    }
}
