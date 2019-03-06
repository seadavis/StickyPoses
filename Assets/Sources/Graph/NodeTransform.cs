using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Assets.Sources.Graph
{

    /// <summary>
    /// Class, that represents how a node transforms,
    /// relative to it's parent.
    /// 
    /// Has a Rotation Component and a translation component.
    /// </summary>
    [Serializable]
    public class NodeTransform
    {
     
        #region Properties

        /// <summary>
        /// the current position of the node.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// the current rotation of the node.
        /// </summary>
        public Tuple<float, Vector3>  Rotation { get;  set;}

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor, Sets both the rotation and position to be
        /// the identity
        /// </summary>
        public NodeTransform()
        {
            this.Position = Vector3.Zero;
            this.Rotation = new Tuple<float, Vector3>(0.0f, Vector3.Zero);
        }

        /// <summary>
        /// Initializes the node transform with the given position and rotation.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public NodeTransform(Vector3 position, Tuple<float, Vector3> rotation) : this()
        {
            this.Position = position;
            this.Rotation = rotation;
        }

        /// <summary>
        /// Sets this potiions of the transform to the given position and leaves the rotation alone.
        /// </summary>
        /// <param name="position"></param>
        public NodeTransform(Vector3 position) : this()
        {
            this.Position = position;
        }

        #endregion

    }
}
