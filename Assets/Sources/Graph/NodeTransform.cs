using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Graph
{
    /// <summary>
    /// Class, that represents how a node transforms,
    /// relative to it's parent.
    /// 
    /// Has a Rotation Component and a translation component.
    /// </summary>
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
        public Quaternion  Rotation { get;  set;}

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor, Sets both the rotation and position to be
        /// the identity
        /// </summary>
        public NodeTransform()
        {
            this.Position = Vector3.zero;
            this.Rotation = Quaternion.identity;
        }

        /// <summary>
        /// Initializes the node transform with the given position and rotation.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public NodeTransform(Vector3 position, Quaternion rotation) : this()
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
