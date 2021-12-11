namespace CsRay
{
    public abstract class Hittable
    {
        public abstract bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec);

        public abstract Aabb BoundingBox(double t0, double t1);
    }
}