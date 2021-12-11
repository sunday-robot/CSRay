using System;

using static CsRay.Util;

namespace CsRay
{
    public sealed class Camera
    {
        public Vec3 LowerLeftCorner { get; }

        public Vec3 Horizontal { get; }

        public Vec3 Vertical { get; }

        /// <summary>視点</summary>
        public Vec3 Origin { get; }

        /// <summary>カメラのX軸方向(単位ベクトル)</summary>
        public Vec3 U { get; }

        /// <summary>カメラのY軸方向(単位ベクトル)</summary>
        public Vec3 V { get; }

        /// <summary>カメラのZ軸方向(単位ベクトル)</summary>
        public Vec3 W { get; }

        /// <summary>レンズのサイズ(ボケを決めるもので、小さいほどボケない。0なら全くボケない。)</summary>
        public double LensRadius { get; }

        public double Time0 { get; }

        public double Time1 { get; }

        /// <param name="lowerLeftCorner"></param>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <param name="origin"></param>
        /// <param name="u">カメラのX軸方向(単位ベクトルであること)</param>
        /// <param name="v">カメラのY軸方向(単位ベクトルであること)</param>
        /// <param name="w">カメラのZ軸方向(単位ベクトルであること)</param>
        /// <param name="lensRadious"></param>
        /// <param name="time0"></param>
        /// <param name="time1"></param>
        public Camera(Vec3 lowerLeftCorner, Vec3 horizontal, Vec3 vertical, Vec3 origin, Vec3 u, Vec3 v, Vec3 w,
            double lensRadious, double time0, double time1)
        {
            LowerLeftCorner = lowerLeftCorner;
            Vertical = vertical;
            Horizontal = horizontal;
            Origin = origin;
            U = u;
            V = v;
            W = w;
            LensRadius = lensRadious;
            Time0 = time0;
            Time1 = time1;
        }

        /// <param name="s">横方向の位置(0～1)</param>
        /// <param name="t">縦方向の位置(0～1)</param>
        public Ray GetRay(double s, double t)
        {
            var rd = LensRadius * RandomInUnitDisk();
            var offset = U * rd.X + V * rd.Y;
            var o = Origin + offset;
            var d = LowerLeftCorner + s * Horizontal + t * Vertical - o;
            var time = Time0 + Rand() * (Time1 - Time0);
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
            double time0,
            double time1)
        {
            var theta = verticalFov * Math.PI / 180.0;
            var halfHeight = Math.Tan(theta / 2.0);
            var halfWidth = aspect * halfHeight;
            var w = (lookAt - lookFrom).Unit;
            var u = -vup.Cross(w).Unit;
            var v = -w.Cross(u);
            var lowerLeftCorner = (lookFrom + focusDist * w
                    - halfWidth * focusDist * u
                    - halfHeight * focusDist * v);
            var horizontal = 2 * halfWidth * focusDist * u;
            var vertical = 2 * halfHeight * focusDist * v;
            var lensRadius = aperture / 2.0;
            return new Camera(lowerLeftCorner, horizontal, vertical, lookFrom, u, v, w, lensRadius, time0, time1);
        }
    }
}
