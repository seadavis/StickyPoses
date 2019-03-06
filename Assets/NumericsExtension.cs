using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    /// <summary>
    /// Set of extensions to use on the numerics library.
    /// </summary>
    public static class NumericsExtension
    {
        /// <summary>
        /// Returns true if the vector has a  NaN component.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool HasNaN(this Vector3 v)
        {
            return float.IsNaN(v.X) || float.IsNaN(v.Y) || float.IsNaN(v.Z);
        }

        /// <summary>
        /// Takes a component wise average of allthe vectors given.
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns>a component wise average of the vectors.</returns>
        public static System.Numerics.Vector3 Average(this IEnumerable<Vector3> vectors)
        {
            float[] x = vectors.Select(v => v.X).ToArray();
            float[] y = vectors.Select(v => v.Y).ToArray();
            float[] z = vectors.Select(v => v.Z).ToArray();

            return new Vector3(x.Average(), y.Average(), z.Average());
        }

        public static UnityEngine.Quaternion ToUnityQuaternion(this System.Numerics.Quaternion q)
        {
            return new UnityEngine.Quaternion(q.X, q.Y, q.Z, q.W);
        }

        public static System.Numerics.Quaternion ToSystemQuaternion(this UnityEngine.Quaternion q)
        {
            return new Quaternion(q.x, q.y, q.w, q.z);
        }

        /// <summary>
        /// Converts a numerics vector to a unity Vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns>the unity vector equivalent.</returns>
        public static UnityEngine.Vector2 ToUnityVector(this System.Numerics.Vector2 v)
        {
            return new UnityEngine.Vector2(v.X, v.Y);
        }

    }
}
