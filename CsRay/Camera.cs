using static CsRay.Util;

namespace CsRay
{
    public sealed class Camera
    {
        readonly Vec3 _lowerLeftCorner;

        readonly Vec3 _horizontal;

        readonly Vec3 _vertical;

        /// <summary>視点</summary>
        readonly Vec3 _origin;

        /// <summary>カメラのX軸方向(単位ベクトル)</summary>
        readonly Vec3 _u;

        /// <summary>カメラのY軸方向(単位ベクトル)</summary>
        readonly Vec3 _v;

        /// <summary>レンズのサイズ(ボケを決めるもので、小さいほどボケない。0なら全くボケない。)</summary>
        readonly double _lensRadius;

        public double ExposureTime { get; }

        /// <param name="lowerLeftCorner"></param>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <param name="origin"></param>
        /// <param name="u">カメラのX軸方向(単位ベクトルであること)</param>
        /// <param name="v">カメラのY軸方向(単位ベクトルであること)</param>
        /// <param name="lensRadious"></param>
        /// <param name="exposureTime">露光時間</param>
        public Camera(Vec3 lowerLeftCorner, Vec3 horizontal, Vec3 vertical, Vec3 origin, Vec3 u, Vec3 v,
            double lensRadious, double exposureTime)
        {
            _lowerLeftCorner = lowerLeftCorner;
            _vertical = vertical;
            _horizontal = horizontal;
            _origin = origin;
            _u = u;
            _v = v;
            _lensRadius = lensRadious;
            ExposureTime = exposureTime;
        }

        /// <param name="s">横方向の位置(0～1)</param>
        /// <param name="t">縦方向の位置(0～1)</param>
        public Ray GetRay(double s, double t)
        {
            var rd = _lensRadius * RandomInUnitDisk();
            var offset = _u * rd.X + _v * rd.Y;
            var o = _origin + offset;
            var d = _lowerLeftCorner + s * _horizontal + t * _vertical - o;
            var time = (Rand() - 0.5) * ExposureTime;
            return new Ray(o, d, time);
        }

        /// <param name="lookFrom">視点</param>
        /// <param name="lookAt">注視点(視線の方向を決めるためだけのもので、ピントがあう場所ではない。ピント位置は、focusDistで指定する。)</param>
        /// <param name="vup">上方向を示すベクトル(視点、注視点のベクトルと同じ方向でなければよい。直交している必要もないし、長さも適当でよい)</param>
        /// <param name="verticalFov">縦方向の視野(角度[°]]</param>
        /// <param name="aspect">縦横比(幅/高さ)</param>
        /// <param name="aperture">絞り(ボケ具体を決めるもの。0なら全くボケない。)</param>
        /// <param name="focusDist">視点からピントが合う位置までの距離</param>
        /// <param name="time0"></param>
        /// <param name="time1"></param>
        public static Camera CreateCamera(
            Vec3 lookFrom,
            Vec3 lookAt,
            Vec3 vup,
            double verticalFov,
            double aspect,
            double aperture,
            double focusDist,
            double exposureTime)
        {
            var theta = verticalFov * Math.PI / 180;
            var halfHeight = Math.Tan(theta / 2);
            var halfWidth = aspect * halfHeight;
            var w = (lookAt - lookFrom).Unit;
            var u = -vup.Cross(w).Unit;
            var v = -w.Cross(u);
            var lowerLeftCorner = (lookFrom + focusDist * w
                    - halfWidth * focusDist * u
                    - halfHeight * focusDist * v);
            var horizontal = 2 * halfWidth * focusDist * u;
            var vertical = 2 * halfHeight * focusDist * v;
            var lensRadius = aperture / 2;
            return new Camera(lowerLeftCorner, horizontal, vertical, lookFrom, u, v, lensRadius, exposureTime);
        }
    }
}
