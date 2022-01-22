namespace CsRay.Hittables
{
    public class RotateY : Hittable
    {
        readonly Hittable _ptr;
        readonly float _sinTheta;
        readonly float _cosTheta;
        readonly Aabb _bbox;

        public RotateY(Hittable p, float angle)
        {
            _ptr = p;
            var radians = angle / 180 * MathF.PI;
            _sinTheta = MathF.Sin(radians);
            _cosTheta = MathF.Cos(radians);
            _bbox = _ptr.BoundingBox(0);

            var minX = float.PositiveInfinity;
            var minY = float.PositiveInfinity;
            var minZ = float.PositiveInfinity;
            var maxX = float.NegativeInfinity;
            var maxY = float.NegativeInfinity;
            var maxZ = float.NegativeInfinity;

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

        public override HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var originX = _cosTheta * ray.Origin.X - _sinTheta * ray.Origin.Z;
            var originY = ray.Origin.Y;
            var originZ = _sinTheta * ray.Origin.X + _cosTheta * ray.Origin.Z;

            var directionX = _cosTheta * ray.Direction.X - _sinTheta * ray.Direction.Z;
            var directionY = ray.Direction.Y;
            var directionZ = _sinTheta * ray.Direction.X + _cosTheta * ray.Direction.Z;

            var rotatedR = new Ray(new Vec3(originX, originY, originZ), new Vec3(directionX, directionY, directionZ), ray.Time);

            var tmpRec = _ptr.Hit(rotatedR, tMin, tMax);
            if (tmpRec == null)
                return null;

            var pX = _cosTheta * tmpRec.Position.X + _sinTheta * tmpRec.Position.Z;
            var pY = tmpRec.Position.Y;
            var pZ = -_sinTheta * tmpRec.Position.X + _cosTheta * tmpRec.Position.Z;

            var normalX = _cosTheta * tmpRec.Normal.X + _sinTheta * tmpRec.Normal.Z;
            var normalY = tmpRec.Normal.Y;
            var normalZ = -_sinTheta * tmpRec.Normal.X + _cosTheta * tmpRec.Normal.Z;

            var p = new Vec3(pX, pY, pZ);
            var n = new Vec3(normalX, normalY, normalZ);
            var ff = ray.Direction.Dot(n) < 0;
            if (!ff)
                n = -n;
            return new HitRecord(tmpRec.T, p, n, tmpRec.Material, ff);
        }

        public override Aabb BoundingBox(float exposureTime) => _bbox;
    }
}
