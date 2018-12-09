using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Sources.Exceptions;
using System.Numerics;
using QuickGraph;


namespace Assets.Sources.Graph
{
    public class CharacterGraph : BidirectionalGraph<CharacterGraphNode, IEdge<CharacterGraphNode>>
    {

        #region PublicMethods

       
        /// <summary>
        /// Finds the node by the given name. Position and rotation,
        /// are always updated to be the latest submitted.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A tuple where the first node is the transform, and the other node matches the given name.</returns>
        public CharacterGraphNode GetNode(string name)
        {
            if (String.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            
            IEnumerable<CharacterGraphNode> graphNodes = this.Vertices
                    .Where((CharacterGraphNode node) => 
                                String.Compare(name, node.Name, StringComparison.InvariantCultureIgnoreCase) == 0); 

            if(graphNodes.Count() > 0)
            {
                CharacterGraphNode node = graphNodes.First();
                node.Transformation.Rotation = this.CalculateRotation(node);
                return node;
            }
            else
            {
                throw new NamedObjectDoesNotExistException(name);
            }
           
        }

        /// <summary>
        /// Grabs all of the edges that sit between the source and target node on the edge.
        /// 
        /// Throws an exception if the two edges are not connected.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>the set of edges that sit between node and node2.</returns>
        public IEdge<CharacterGraphNode>[] GetEdges(CharacterGraphEdge edge)
        {
            List<IEdge<CharacterGraphNode>> edges = new List<IEdge<CharacterGraphNode>>(this.EdgeCount);
            IEdge<CharacterGraphNode> currentEdge;
            CharacterGraphNode node = this.GetNode(edge.Target.Name);

            while((currentEdge = this.GetParentEdge(node)) != null && !currentEdge.Target.Equals(edge.Source))
            {
                edges.Add(currentEdge);
                node = currentEdge.Source;
            }
              
            if(node != null && !node.Equals(edge.Source))
            {
                throw new EdgesNotConnectedException(edge.Source, edge.Target);
            }

            edges.Reverse();
            return edges.ToArray();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Grabs the 4x4 rotation matrix of the given graph node.
        /// </summary>
        /// <param name="node">the node we want to calculate the rotation for. </param>
        /// <returns>the rotation for the given node.</returns>
        protected virtual Quaternion CalculateRotation(CharacterGraphNode node)
        {
            IEnumerable<CharacterGraphNode> graphNode = this.Vertices.Where(c => c.Equals(node));

            if (graphNode.Count() == 0)
            {
                throw new NamedObjectDoesNotExistException(node);
            }
            else
            {
                CharacterGraphNode parent = this.GetParent(node);
                IEdge<CharacterGraphNode> child = this.GetChildEdge(node);
                if (parent != null && child != null)
                {
                    return this.CalculateRotation(parent, graphNode.First(), child.Target);
                }
                else
                {
                    return graphNode.First().Transformation.Rotation;
                }
            }
        }

     
        #endregion

        #region Private Methods

        /// <summary>
        /// Grabs the edge that has the given node as the target,
        /// or NULL if not such edge exists.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>NULL if the node is not a target on any of the edges, or the parent edge.</returns>
        private IEdge<CharacterGraphNode> GetParentEdge(CharacterGraphNode node)
        {
            IEnumerable<IEdge<CharacterGraphNode>> edges = this.Edges.Where((edge) => edge.Target.Equals(node));
            if (edges.Count() > 0)
            {
                IEdge<CharacterGraphNode> edge = edges.First();
                return edge;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Grabs the edge where the node is the Source.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>the edge where this points to the child.</returns>
        private IEdge<CharacterGraphNode> GetChildEdge(CharacterGraphNode node)
        {
            
            IEnumerable<IEdge<CharacterGraphNode>> edges = this.Edges.Where((edge) => edge.Source.Equals(node));
            if (edges.Count() > 0)
            {
                IEdge<CharacterGraphNode> edge = edges.First();
                return edge;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the parent of the current node.
        /// </summary>
        /// <param name="node">the whose parent we want to find.</param>
        /// <returns>the parent of the node, or null if no such parent exists.</returns>
        private CharacterGraphNode GetParent(CharacterGraphNode node)
        {
            IEdge<CharacterGraphNode> edge = this.GetParentEdge(node);

            if(edge != null)
            {
                return edge.Source;
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// Calculates the rotation between the vertices from node to node2 and node2 to node3
        /// </summary>
        /// <param name="node">the first node in the chain.</param>
        /// <param name="node2">the second node in the change</param>
        /// <param name="node3">the last node in the chain.</param>
        /// <returns>a quaternion representing the rotation between the three nodes.</returns>
        private Quaternion CalculateRotation(CharacterGraphNode node, CharacterGraphNode node2, CharacterGraphNode node3)
        {
            Vector3 v1 = Vector3.Normalize(node2.Transformation.Position - node.Transformation.Position);
            Vector3 v2 = Vector3.Normalize(node3.Transformation.Position - node2.Transformation.Position);
            float angle = Vector3.Dot(v1, v2);
            Vector3 axis = Vector3.Normalize(Vector3.Cross(v1, v2));

           

            return Quaternion.CreateFromAxisAngle(axis, angle);
        }

        #endregion

    }
}
