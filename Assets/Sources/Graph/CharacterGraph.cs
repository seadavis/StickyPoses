using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using UnityEngine;

namespace Assets.Sources.Graph
{
    public class CharacterGraph : BidirectionalGraph<CharacterGraphNode, IEdge<CharacterGraphNode>>
    {

        #region PublicMethods

       
        /// <summary>
        /// Finds the node by the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A tuple where the first node is the transform, and the other node matches the given name.</returns>
        public CharacterGraphNode GetNode(string name)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// Grabs all of the edges that sit between the source and target node on the edge.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>the set of edges that sit between node and node2.</returns>
        public IEdge<CharacterGraphNode>[] GetEdges(CharacterGraphEdge edge)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Grabs the 4x4 rotation matrix of the given graph node.
        /// </summary>
        /// <param name="node">the node we want to calculate the rotation for. </param>
        /// <returns>the rotation for the given node.</returns>
        protected virtual UnityEngine.Quaternion CalculateRotaton(CharacterGraphNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks to ensure no node with the same name as c as already been added.
        /// </summary>
        /// <param name="c">the new character graph node being added.</param>
        protected override void OnVertexAdded(CharacterGraphNode c)
        {
            throw new NotImplementedException();
        }
        

        #endregion

    }
}
