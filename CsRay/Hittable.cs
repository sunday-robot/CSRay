namespace CsRay
{
    public abstract class Hittable
    {
        public abstract HitRecord? Hit(Ray ray, double tMin, double tMax);

        public abstract Aabb BoundingBox(double dt);
    }
}