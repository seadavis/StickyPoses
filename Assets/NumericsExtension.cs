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
        /// Takes a component wise average of allthe vectors given.
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns>a component wise average of the vectors.</returns>
        public static Vector3 Average(this IEnumerable<Vector3> vectors)
        {
            float[] x = vectors.Select(v => v.X).ToArray();
            float[] y = vectors.Select(v => v.Y).ToArray();
            float[] z = vectors.Select(v => v.Z).ToArray();

            return new Vector3(x.Average(), y.Average(), z.Average());
        }

        /// <summary>
        /// Converts a numerics vector to a unity Vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns>the unity vector equivalent.</returns>
        public static UnityEngine.Vector2 ToUnityVector(this Vector2 v)
        {
            return new UnityEngine.Vector2(v.X, v.Y);
        }

    }
}
