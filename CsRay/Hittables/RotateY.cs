using System;

namespace CsRay.Hittables
{
    public class RotateY : Hittable
    {
        readonly Hittable _ptr;
        readonly double _sinTheta;
        readonly double _cosTheta;
        readonly Aabb _bbox;

        public RotateY(Hittable p, double angle)
        {
            _ptr = p;
            var radians = angle / 180 * Math.PI;
            _sinTheta = Math.Sin(radians);
            _cosTheta = Math.Cos(radians);
            _bbox = _ptr.BoundingBox(0, 1);

            var minX = double.PositiveInfinity;
            var minY = double.PositiveInfinity;
            var minZ = double.PositiveInfinity;
            var maxX = double.NegativeInfinity;
            var maxY = double.NegativeInfinity;
            var maxZ = double.NegativeInfinity;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var x = i * _bbox.Max.X + (1 - i) * _bbox.Min.X;
                        var y = j * _bbox.Max.Y + (1 - j) * _bbox.Min.Y;
                        var z = k * _bbox.Max.Z + (1 - k) * _bbox.Min.Z;

                        var newx = _cosTheta * x + _sinTheta * z;
                        var newz = -_sinTheta * x + _cosTheta * z;

                        minX = Math.Min(minX, newx);
                        maxX = Math.Max(maxX, newx);
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);
                        minZ = Math.Min(minZ, newz);
                        maxZ = Math.Max(maxZ, newz);
                    }
                }
            }

            _bbox = new Aabb(new Vec3(minX, minY, minZ), new Vec3(maxX, maxY, maxZ));
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            var originX = _cosTheta * ray.Origin.X - _sinTheta * ray.Origin.Z;
            var originY = ray.Origin.Y;
            var originZ = _sinTheta * ray.Origin.X + _cosTheta * ray.Origin.Z;

            var directionX = _cosTheta * ray.Direction.X - _sinTheta * ray.Direction.Z;
            var directionY = ray.Direction.Y;
            var directionZ = _sinTheta * ray.Direction.X + _cosTheta * ray.Direction.Z;

            var rotatedR = new Ray(new Vec3(originX, originY, originZ), new Vec3(directionX, directionY, directionZ), ray.Time);

            if (!_ptr.Hit(rotatedR, tMin, tMax, ref rec))
                return false;

            var pX = _cosTheta * rec.Position.X + _sinTheta * rec.Position.Z;
            var pY = rec.Position.Y;
            var pZ = -_sinTheta * rec.Position.X + _cosTheta * rec.Position.Z;

            var normalX = _cosTheta * rec.Normal.X + _sinTheta * rec.Normal.Z;
            var normalY = rec.Normal.Y;
            var normalZ = -_sinTheta * rec.Normal.X + _cosTheta * rec.Normal.Z;

            rec.SetPosition(new Vec3(pX, pY, pZ));
            rec.SetFaceNormal(rotatedR, new Vec3(normalX, normalY, normalZ));

            return true;
        }

        public override Aabb BoundingBox(double t0, double t1) => _bbox;
    }
}
