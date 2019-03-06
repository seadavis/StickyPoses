using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Sources.Exceptions;
using System.Numerics;
using QuickGraph;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Assets.Sources.Graph
{
    [Serializable]
    public class CharacterGraph : BidirectionalGraph<CharacterGraphNode, CharacterGraphEdge>
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
        /// Calculatesall of the rotations on all of the nodes on the character graph.
        /// </summary>
        public void CalculateRotations()
        {
            foreach(CharacterGraphNode vertex in this.Vertices)
            {
                vertex.Transformation.Rotation = this.CalculateRotation(vertex);
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
        /// <param name="node">the edge to caluclatethe rotation for.</param>
        /// <returns>the rotation for the given node.</returns>
        protected virtual Tuple<float, Vector3> CalculateRotation(CharacterGraphNode node)
        {
            IEnumerable<CharacterGraphNode> graphNode = this.Vertices.Where(c => c.Equals(node));

            if (graphNode.Count() == 0)
            {
                throw new NamedObjectDoesNotExistException(node);
            }
            else
            {
             
                CharacterGraphEdge edge = this.GetChildEdge(node);

                if (edge != null)
                {
                    return this.CalculateRotation(edge);
                }
                else
                {
                    return graphNode.First().Transformation.Rotation;
                }
            }
        }



        #endregion

        #region Static Methods

        /// <summary>
        /// De serializes the JSON serialization of the given graph.
        /// </summary>
        /// <param name="serializedGraph">the JSON serialization of the graph.</param>
        /// <returns>The character graph represented in the given serialization</returns>
        public static CharacterGraph DeSerializeGraph(string serializedGraph)
        {
            JObject obj = JObject.Parse(serializedGraph);
            CharacterGraphNode[] nodes = obj["Vertices"].ToObject<CharacterGraphNode[]>();
            CharacterGraphEdge[] edges = obj["Edges"].ToObject<CharacterGraphEdge[]>();
            CharacterGraph graph = new CharacterGraph();
            foreach (CharacterGraphEdge e in edges)
            {
                CharacterGraphNode source;
                CharacterGraphNode target;
                CharacterGraphNode[] sourceInGraph = graph.Vertices.Where(v => v.Name == e.Source.Name).ToArray();
                CharacterGraphNode[] targetInGraph = graph.Vertices.Where(v => v.Name == e.Target.Name).ToArray();

                if (sourceInGraph.Length == 0)
                {
                    graph.AddVertex(e.Source);
                    source = e.Source;
                }
                else
                {
                    source = sourceInGraph.First();
                }

                if (targetInGraph.Length == 0)
                {
                    graph.AddVertex(e.Target);
                    target = e.Target;
                }
                else
                {
                    target = targetInGraph.First();
                }
                CharacterGraphEdge edge = new CharacterGraphEdge(source, target);
                edge.NeutralDirection = e.NeutralDirection;
                edge.Source.NeutralRotation = e.Source.NeutralRotation;
                edge.Target.NeutralRotation = e.Target.NeutralRotation;
                edge.AngleRange = e.AngleRange;
                graph.AddEdge(edge);
            }

            return graph;
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
        private CharacterGraphEdge GetChildEdge(CharacterGraphNode node)
        {
            
            IEnumerable<CharacterGraphEdge> edges = this.Edges.Where((edge) => edge.Source.Equals(node));
            if (edges.Count() > 0)
            {
                CharacterGraphEdge edge = edges.First();
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
        /// <param name="edge">the edge to calculate the rotation of.</param>
        /// <returns>a quaternion representing the rotation between the three nodes.</returns>
        private Tuple<float,Vector3> CalculateRotation(CharacterGraphEdge edge)
        {
           
            Vector3 v1 = Vector3.Normalize(edge.Target.Transformation.Position - edge.Source.Transformation.Position);
            Vector3 v2 = Vector3.Normalize(edge.NeutralDirection);
            /*CharacterGraphEdge parentEdge = this.Edges.Where(e => e.Target.Name == edge.Source.Name).FirstOrDefault();
            if (parentEdge != null)
            {
                Tuple<float, Vector3> parentRotation = parentEdge.Source.Transformation.Rotation;
                if (parentRotation.Item1 > 0)
                {
                    Quaternion axisAngle = Quaternion.CreateFromAxisAngle(parentRotation.Item2, parentRotation.Item1 * (float)(Math.PI / 180.0));
                    Vector3 newNeutral = Vector3.Normalize(Vector3.Transform(v2, axisAngle));
                    v2 = newNeutral;

                    if(edge.Source.Name == "rightElbow")
                    {
                        UnityEngine.Debug.Log(
                     string.Format("Source: {0} Target:{1} V1:{2} V2:{3} Neutral Direction:{4}, Parent Angle: {5}, Parent Axis:{6} Parent Q:{7}",
                     edge.Source.Name, edge.Target.Name, v1, v2, edge.NeutralDirection, parentRotation.Item1, parentRotation.Item2, axisAngle));
                    }
                 
                }

              
            }*/
            float dot = Vector3.Dot(v1, v2);
            float acos = (float)(Math.Acos(dot));
            float angle = (float)(acos * (180.0f / Math.PI));

            Vector3 axis = Vector3.Normalize(Vector3.Cross(v1, v2));

            if (edge.Source.Name == "rightElbow")
            {
                UnityEngine.Debug.Log(string.Format("Calculated Axis: {0} Calculated Angle:{1} Dot: {2} ACos:{3}", axis, angle,dot, acos));
            }

            if (axis.HasNaN())
            {
                return new Tuple<float, Vector3>(0.0f, Vector3.UnitX);
            }
            else
            {
              
                return new Tuple<float, Vector3>(angle,  axis);
            }
            
        }

        #endregion

    }
}
