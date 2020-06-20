using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

    public struct OrthonormalBasis
    {
        private OrthonormalBasis(in Vector3 u, in Vector3 v, in Vector3 w)
        {
            this.u = u;
            this.v = v;
            this.w = w;
        }

        public Vector3 local(float x, float y, float z)
        {
            return x * u + y * v + z * w;
        }

        public Vector3 local(in Vector3 vec)
        {
            return vec.X * u + vec.Y * v + vec.Z * w;
        }

        static public OrthonormalBasis CreateFromW(in Vector3 vec)
        {
            var w = Vector3.Normalize(vec);
            var right = Math.Abs(w.X) > 0.9f ? Vector3.UnitY : Vector3.UnitX;
            var v = Vector3.Normalize(Vector3.Cross(w, right));
            var u = Vector3.Cross(w, v);
            return new OrthonormalBasis(u, v, w);
        }

        public Vector3 u { get; }
        public Vector3 v { get; }
        public Vector3 w { get; }
    }

}
