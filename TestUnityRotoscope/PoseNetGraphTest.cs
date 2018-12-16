using System;
using Assets.Sources.Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace TestUnityRotoscope
{
    [TestClass]
    public class PoseNetGraphTest
    {
        [TestMethod]
        public void UpdateSingleKeyPoint()
        {
            

        }

        [TestMethod]
        public void UpdateMultipleKeyPoints()
        {
            
        }
    

        [TestMethod]
        public void UpdateOneToOne()
        {

        }

        [TestMethod]
        public void UpdateManyToOne()
        {

        }

        /// <summary>
        /// Sets up a 9 node test graph with enough complexity to test some
        /// of the more common cases.
        /// </summary>
        /// <returns></returns>
        public PoseNetGraph TestGraph()
        {

            PoseNetGraph graph = new PoseNetGraph();
            CharacterGraphNode n1 = new CharacterGraphNode(new NodeTransform(), "n1");
            CharacterGraphNode n2 = new CharacterGraphNode(
                                        new NodeTransform(), "n2");

            CharacterGraphNode n3 = new CharacterGraphNode(
                                        new NodeTransform(), "n3");

            CharacterGraphNode n4 = new CharacterGraphNode(
                                        new NodeTransform(), "n4");

            CharacterGraphNode n5 = new CharacterGraphNode(
                                     new NodeTransform(), "n5");

            CharacterGraphNode n6 = new CharacterGraphNode(
                                     new NodeTransform(), "n6");

            CharacterGraphNode n7 = new CharacterGraphNode(
                                     new NodeTransform(), "n7");

            CharacterGraphNode n8 = new CharacterGraphNode(
                                  new NodeTransform(), "n8");

            CharacterGraphNode n9 = new CharacterGraphNode(
                                 new NodeTransform(), "n9");


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
