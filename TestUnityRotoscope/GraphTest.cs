using System;
using Assets.Sources.Exceptions;
using Assets.Sources.Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph;
using System.Numerics;
using System.Collections.Generic;

namespace TestUnityRotoscope
{
    [TestClass]
    public class GraphTest
    {
        [TestMethod]
        public void Equality()
        {
            CharacterGraphNode n1 = new CharacterGraphNode(new NodeTransform(), "name");
            CharacterGraphNode n2 = new CharacterGraphNode(new NodeTransform(), "name");
            Assert.AreEqual(true, n1.Equals(n2));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetNodeEmptyString()
        {

            CharacterGraph graph = new CharacterGraph();


            graph.AddEdge(new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "Hello"),
                                                new CharacterGraphNode(new NodeTransform(), "goodbye")));
            graph.GetNode("");
            
        }

        [TestMethod]
        [ExpectedException(typeof(EdgesNotConnectedException))]
        public void GetEdgesNotConnected()
        {
            CharacterGraph graph = this.TestGraph();

            var n1 = new CharacterGraphNode(new NodeTransform(), "hello");
            var n2 = new CharacterGraphNode(new NodeTransform(), "goodbye");
            graph.AddVertex(n1);
            graph.AddVertex(n2);
            graph.AddEdge(new CharacterGraphEdge(n1, n2));

            graph.GetEdges(new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "n1"),
                                                new CharacterGraphNode(new NodeTransform(), "goodbye")));
        }

        [TestMethod]
        [ExpectedException(typeof(NamedObjectDoesNotExistException))]
        public void GetNodeNonExistent()
        {

            CharacterGraph graph = new CharacterGraph();

            var n1 = new CharacterGraphNode(new NodeTransform(), "Hello");
            var n2 = new CharacterGraphNode(new NodeTransform(), "goodbye");
            graph.AddVertex(n1);
            graph.AddVertex(n2);
            graph.AddEdge(new CharacterGraphEdge(n1,n2));
            graph.GetNode("maybe");

        }

       

        [TestMethod]
        public void GetSingleNodeGraph()
        {
            CharacterGraph graph = new CharacterGraph();
            graph.AddVertex(new CharacterGraphNode(new NodeTransform(), "goodbye"));

            CharacterGraphNode n = graph.GetNode("goodbye");
            Assert.AreEqual(n.Transformation.Position, Vector3.Zero);
            Assert.AreEqual(n.Transformation.Rotation, Quaternion.Identity);
        }

        [TestMethod]
        public void GetNodeRoot()
        {

            CharacterGraph graph = this.TestGraph();
            CharacterGraphNode root = graph.GetNode("n1");
            Assert.AreEqual(Vector3.Zero, root.Transformation.Position);
            Assert.AreEqual(Quaternion.Identity, root.Transformation.Rotation);
            Assert.AreEqual("n1", root.Name);
        }

        [TestMethod]
        public void GetNodesWithRotations()
        {
            CharacterGraph graph = this.TestGraph();
            CharacterGraphNode n2 = graph.GetNode("n2");
            Assert.AreEqual("n2", n2.Name);
            Assert.AreEqual(new Vector3(0, -1.0f, 0), n2.Transformation.Position);

            Vector3 v1 = Vector3.Normalize(new Vector3(0.5f, -0.25f, 0.0f) -
                                        new Vector3(0.0f, -1.0f, 0.0f));
            Vector3 v2 = new Vector3(0.0f, -1.0f, 0.0f);
            Assert.AreEqual(Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1.0f), Vector3.Dot(v1, v2)), n2.Transformation.Rotation);
           

            
           

        }

        [TestMethod]
        public void GetBottomNode()
        {
            CharacterGraph graph = this.TestGraph();
            CharacterGraphNode n5 = graph.GetNode("n5");
            Assert.AreEqual("n5", n5.Name);
           
            Assert.AreEqual(Quaternion.Identity, n5.Transformation.Rotation);
        }

        [TestMethod]
        public void GetBetweenOne()
        {
            CharacterGraph graph = this.TestGraph();
            CharacterGraphNode n1 = graph.GetNode("n1");
            CharacterGraphNode n3 = graph.GetNode("n3");
            IEdge<CharacterGraphNode>[] edges = graph.GetEdges(new CharacterGraphEdge(n1, n3));
            Assert.AreEqual(2, edges.Length);

            IEdge<CharacterGraphNode> edge = edges[0];
            Assert.AreEqual("n1", edge.Source.Name);
            Assert.AreEqual("n2", edge.Target.Name);

            edge = edges[1];
            Assert.AreEqual("n2", edge.Source.Name);
            Assert.AreEqual("n3", edge.Target.Name);

        }

        [TestMethod]
        public void GetBetweenNone()
        {
            CharacterGraph graph = this.TestGraph();
            CharacterGraphNode n7 = graph.GetNode("n7");
            CharacterGraphNode n8 = graph.GetNode("n8");
            IEdge<CharacterGraphNode>[] edges = graph.GetEdges(new CharacterGraphEdge(n7, n8));
            Assert.AreEqual(1, edges.Length);

            IEdge<CharacterGraphNode> edge = edges[0];
            Assert.AreEqual("n7", edge.Source.Name);
            Assert.AreEqual("n8", edge.Target.Name);

          
        }

        [TestMethod]
        public void GetBetweenMultiple()
        {
            CharacterGraph graph = this.TestGraph();
            CharacterGraphNode start = graph.GetNode("n1");
            CharacterGraphNode end = graph.GetNode("n5");
            IEdge<CharacterGraphNode>[] edges = graph.GetEdges(new CharacterGraphEdge(start, end));
            Assert.AreEqual(4, edges.Length);

            IEdge<CharacterGraphNode> edge = edges[0];
            Assert.AreEqual("n1", edge.Source.Name);
            Assert.AreEqual("n2", edge.Target.Name);

            edge = edges[1];
            Assert.AreEqual("n2", edge.Source.Name);
            Assert.AreEqual("n3", edge.Target.Name);

            edge = edges[2];
            Assert.AreEqual("n3", edge.Source.Name);
            Assert.AreEqual("n4", edge.Target.Name);

            edge = edges[3];
            Assert.AreEqual("n4", edge.Source.Name);
            Assert.AreEqual("n5", edge.Target.Name);

        }

        /// <summary>
        /// Sets up a 9 node test graph with enough complexity to test some
        /// of the more common cases.
        /// </summary>
        /// <returns></returns>
        public CharacterGraph TestGraph()
        {

            CharacterGraph graph = new CharacterGraph();
            CharacterGraphNode n1 = new CharacterGraphNode(new NodeTransform(), "n1");
            CharacterGraphNode n2 = new CharacterGraphNode(
                                        new NodeTransform(new Vector3(0, -1.0f, 0)), "n2");

            CharacterGraphNode n3 = new CharacterGraphNode(
                                        new NodeTransform(new Vector3(0.50f, -0.25f, 0)), "n3");

            CharacterGraphNode n4 = new CharacterGraphNode(
                                        new NodeTransform(new Vector3(0.25f, -1.2f, 0)), "n4");

            Quaternion q = Quaternion.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), 45);

            Vector3 p = Vector3.Transform(new Vector3(-0.75f, -1.0f, 0.0f), q);

            CharacterGraphNode n5 = new CharacterGraphNode(
                                     new NodeTransform(p), "n5");

            CharacterGraphNode n6 = new CharacterGraphNode(
                                     new NodeTransform(p), "n6");

            CharacterGraphNode n7 = new CharacterGraphNode(
                                     new NodeTransform(new Vector3(p.X - 0.5f, p.Y -2.5f, p.Z)), "n7");

            CharacterGraphNode n8 = new CharacterGraphNode(
                                  new NodeTransform(new Vector3(p.X - 0.25f, p.Y -3.1f, p.Z)), "n8");

            CharacterGraphNode n9 = new CharacterGraphNode(
                                 new NodeTransform(new Vector3(p.Z - 0.01f, p.Y -3.7f, p.Z)), "n9");

            graph.AddVertex(n1);
            graph.AddVertex(n2);
            graph.AddVertex(n3);
            graph.AddVertex(n4);
            graph.AddVertex(n5);
            graph.AddVertex(n6);
            graph.AddVertex(n7);
            graph.AddVertex(n8);
            graph.AddVertex(n9);
            graph.AddEdge(new CharacterGraphEdge(n1, n2));
            graph.AddEdge(new CharacterGraphEdge(n2, n3));
            graph.AddEdge(new CharacterGraphEdge(n3, n4));
            graph.AddEdge(new CharacterGraphEdge(n4, n5));
            graph.AddEdge(new CharacterGraphEdge(n2, n6));
            graph.AddEdge(new CharacterGraphEdge(n6, n7));
            graph.AddEdge(new CharacterGraphEdge(n7, n8));
            graph.AddEdge(new CharacterGraphEdge(n8, n9));


            return graph;
        }
    }
}
