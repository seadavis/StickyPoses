using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Exceptions
{
    public class AxisNotNormalizedException : Exception
    {

        public AxisNotNormalizedException(Vector3 axis) : base(String.Format("The axis: ({0}, {1}, {2}) is not normalized", axis.x, axis.y, axis.z))
        {

        }
    }
}
