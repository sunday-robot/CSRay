namespace CsRay
{
    public abstract class Hittable
    {
        public abstract HitRecord Hit(Ray ray, double tMin, double tMax, HitRecord rec);

        public abstract Aabb BoundingBox(double t0, double t1);
    }
}