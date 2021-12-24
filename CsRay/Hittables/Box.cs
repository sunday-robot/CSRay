using CsRay.Materials;

namespace CsRay.Hittables
{
    public sealed class Box : Hittable
    {
        readonly HittableList _sides;
        readonly Aabb _aabb;

        public Box(Vec3 p0, Vec3 p1, Material material)
        {
            _sides = new HittableList(new Hittable[] {
                new XyRect(p0.X, p0.Y, p1.X, p1.Y, p1.Z, material),
                new XyRect(p0.X, p0.Y, p1.X, p1.Y, p0.Z, material),
                new XzRect(p0.X, p0.Z, p1.X, p1.Z, p1.Y, material),
                new XzRect(p0.X, p0.Z, p1.X, p1.Z, p0.Y, material),
                new YzRect(p0.Y, p0.Z, p1.Y, p1.Z, p1.X, material),
                new YzRect(p0.Y, p0.Z, p1.Y, p1.Z, p0.X, material),
                });
            _aabb = new Aabb(p0, p1);
        }

        public override Aabb BoundingBox(double t0, double t1) => _aabb;

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec) => _sides.Hit(ray, tMin, tMax, ref rec);
    }
}
