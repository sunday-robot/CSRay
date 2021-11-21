﻿using System;

namespace CsRay
{
    /// <summary>
    /// 素材の基底クラス
    /// </summary>
    public abstract class Material
    {
        public Rgb Emitted(double u, double v, Vec3 p)
        {
            return new Rgb(0, 0, 0);
        }

        /// <param name="ray">レイ</param>
        /// <returns>衝突点で分散(？)されたレイ</returns>
        public abstract (Rgb, Ray)? Scatter(Ray ray, HitRecord rec);

        /// <param name="v">入射ベクトル</param>
        /// <param name="normal">法線ベクトル</param>
        /// <returns>反射ベクトル</returns>
        public Vec3 Reflect(Vec3 v, Vec3 normal) => v - 2 * v.Dot(normal) * normal;

        /// <param name="v">入射ベクトル</param>
        /// <param name="normal">法線ベクトル</param>
        /// <param name="niOverNt">?</param>
        /// <returns>屈折ベクトル(屈折しない場合はnull)</returns>
        public Vec3 Refract(Vec3 v, Vec3 normal, double niOverNt)
        {
            var uv = v.Unit;
            var dt = uv.Dot(normal);
            var discriminant = 1.0 - niOverNt * niOverNt * (1.0 - dt * dt);
            if (discriminant <= 0.0)
            {
                return null;
            }
            return niOverNt * (uv - normal * dt) - normal * Math.Sqrt(discriminant);
        }
    }
}