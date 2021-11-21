using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsRay
{
    public class Translate : Hittable
    {
        readonly Hittable _ptr;

        readonly Vec3 _offset;

        public Translate(Hittable p, Vec3 displacement)
        {
            _ptr = p;
            _offset = displacement;
        }

        public override HitRecord Hit(Ray r, double t_min, double t_max, HitRecord rec)
        {
            var movedR = new Ray(r.Origin - _offset, r.Direction, r.Time);
            var hr = _ptr.Hit(movedR, t_min, t_max, rec);
            if (hr == null)
                return null;
            hr = new HitRecord(rec.T, rec.Position + _offset, rec.Normal, rec.Material);
            hr.SetFaceNormal(movedR, rec.Normal);
            return hr;
        }

        public override Aabb BoundingBox(double time0, double time1)
        {
            var b = _ptr.BoundingBox(time0, time1);
            if (b == null)
                return null;

            return new Aabb(
                b.Min + _offset,
                b.Max + _offset);
        }
    }
}
