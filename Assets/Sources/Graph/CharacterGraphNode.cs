using Assets.Sources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Graph
{
    public class CharacterGraphNode : INamedObject
    {

        #region Properties

        /// <summary>
        /// The name of the node, used for equality checks.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The transformation specified on the given character graph node. 
        /// Used to transform all children edges relative to their parent.
        /// </summary>
        public NodeTransform Transformation
        {
            get;
            private set;
        }

        #endregion

        #region Constructors
        /// <summary>
        /// sets up a node with the identity transform and an empty name.
        /// </summary>
        public CharacterGraphNode()
        {
            this.Name = string.Empty;
            this.Transformation = new NodeTransform();
        }

        /// <summary>
        /// The node for a character graph, with the given name.
        /// </summary>
        /// <param name="transform">the transform to use on a graph.</param>
        /// <param name="name">the name of the node.</param>
        public CharacterGraphNode(NodeTransform transform, string name)
        {
            this.Name = name;
            this.Transformation = transform;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests if the two nodes have the same name.
        /// </summary>
        /// <param name="node">the node we want to compare to.</param>
        /// <returns>True if the names are equal, ignoring case.</returns>
        public bool Equals(CharacterGraphNode node)
        {
            return String.Compare(this.Name, node.Name, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        #endregion

    }
}
