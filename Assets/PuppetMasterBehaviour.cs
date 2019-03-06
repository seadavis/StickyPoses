using Assets.Sources.Graph;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Sources.Maps;
using Assets.Sources.Graph;
using System.Linq;
using UnityEditor;

public class PuppetMasterBehaviour : MonoBehaviour {

   
    static int tick = 0;
    CharacterGraph graph;
    GameObject soldier;
    GraphMap characterMap;
    KeyPointMap keyPointMap;
    WebCameraExample webCam;
    Transform headTransform;

	// Use this for initialization
	void Start () {
        soldier = GameObject.Find("Sci-Fi_Soldier");
        characterMap = CharacterGraphMap();
        TextAsset graphSerialization = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/CharGraph.json", typeof(TextAsset));
        graph = CharacterGraph.DeSerializeGraph(graphSerialization.text);
        characterMap.Update(graph.Edges.Cast<CharacterGraphEdge>().ToList());
        keyPointMap = CreateKeyPointMap(Vector3.zero);
        webCam = GameObject.FindObjectOfType<WebCameraExample>();
     
      
    }
	
	// Update is called once per frame
	void Update () {

        if (graph != null && webCam != null && webCam.Keypoints.Count>0)
        {
            PoseNet.Keypoint[] keypoints;
            
            if(webCam.Keypoints.TryDequeue(out keypoints))
            {
                List<PoseNet.Keypoint> points = new List<PoseNet.Keypoint>(keypoints);
                keyPointMap.Update(points);
                CharacterGraphNode[] updatedNodes = keyPointMap.GetOuput(points);
                List<CharacterGraphNode> seenNodes = new List<CharacterGraphNode>();
               
                if(updatedNodes != null)
                {
                    foreach (CharacterGraphNode updatedNode in updatedNodes)
                    {
                        CharacterGraphNode node = graph.GetNode(updatedNode.Name);
                        node.Transformation.Position = new System.Numerics.Vector3(updatedNode.Transformation.Position.X, updatedNode.Transformation.Position.Y, updatedNode.Transformation.Position.Z );
                    }

                    graph.CalculateRotations();
                    characterMap.Update(graph.Edges.Cast<CharacterGraphEdge>().ToList());
                    UpdateCharacter(graph, soldier, AlignRig);
                }
                

            }
        }
       
	}

   
    

    /// <summary>
    /// Makes the transform match the node.
    /// </summary>
    /// <param name="node">the node we want to align the transform to.</param>
    /// <param name="source">the transform we want to align to the source node of the edge.</param>
    void AlignRig(CharacterGraphEdge edge, Transform source, Transform target)
    {
      
        CharacterGraphNode node = edge.Source;
        Quaternion neutralRotation = new Quaternion(edge.Source.NeutralRotation.X,
                                                    edge.Source.NeutralRotation.Y,
                                                    edge.Source.NeutralRotation.Z,
                                                    edge.Source.NeutralRotation.W);
        
        Quaternion[] parentQuaternions = this.GetParentRotations(source);      
        Quaternion totalRotation = this.CalculateRotation(parentQuaternions,false);

        Vector3 axis = new Vector3( node.Transformation.Rotation.Item2.X,
                                    node.Transformation.Rotation.Item2.Y,
                                    node.Transformation.Rotation.Item2.Z) ;
        float angle = node.Transformation.Rotation.Item1;

        Vector3 newAxis = totalRotation*axis;
        Quaternion rotation = Quaternion.AngleAxis(angle, newAxis);

        float finishedAngle;
        Vector3 finishedAxis;
        rotation.ToAngleAxis(out finishedAngle, out finishedAxis);
        source.localRotation = rotation;

    }

    /// <summary>
    /// Maps angle in the from range to an angle in the to range.
    /// </summary>
    /// <param name="from">the from range.</param>
    /// <param name="to">the range we are mapping to</param>
    /// <param name="angle">the angle we are mapping to.</param>
    /// <returns></returns>
    float MapAxisRange(Tuple<float, float> from, Tuple<float,float> to, float angle)
    {
        return (angle - from.Item1) * (to.Item2 - to.Item1) / (from.Item2 - from.Item1);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node">the node to update in a circle.</param>
    /// <param name="tick">the tick used to measure the how far along the circle we are.</param>
    void MoveCircle(CharacterGraphNode node, int tick)
    {
        node.Transformation.Position = new System.Numerics.Vector3((float)Math.Cos((double)tick/100* 0.261799), 0.0f, (float)Math.Sin((double)tick / 100 * 0.261799));
       
    }

    Quaternion CalculateRotation(Quaternion[] parentQuaternions,bool print)
    {
        Quaternion cumulativeQuaternions = Quaternion.identity;
        Quaternion currentQuaternion = Quaternion.identity;
        foreach (Quaternion q in parentQuaternions)
        {
            Vector3 axis;
            float angle;
            q.ToAngleAxis(out angle, out axis);
            if(print)
            {
                Debug.Log(String.Format("Parent Angle:{0} Parent Axis:{1}, current Quaternion: {2} Cumulative Quaternion: {3} ",
                    angle, axis, currentQuaternion, cumulativeQuaternions));
            }
            currentQuaternion = Quaternion.AngleAxis(angle, cumulativeQuaternions * axis);
            cumulativeQuaternions = cumulativeQuaternions *currentQuaternion;
        }

        return currentQuaternion;
    }

    Quaternion[] GetParentRotations(Transform t)
    {
        List<Quaternion> quaternions = new List<Quaternion>();
        Transform currentTransform = t;

        // start at the grandparent not the parent
        while(currentTransform.parent !=null)
        {
            Vector3 axis;
            float angle;
            currentTransform.parent.rotation.ToAngleAxis(out angle, out axis);

            if(angle >0.00001)
            {
                quaternions.Add(currentTransform.parent.rotation);
            }

            currentTransform = currentTransform.parent;
        }
            

        quaternions.Reverse();
        return quaternions.ToArray();
    }


    /// <summary>
    /// Ensures the solider and the graph match the action.
    /// </summary>
    /// <param name="graph">the graph that uniquely identifies the soldiers positions.</param>
    /// <param name="soldier">the soldier being positioned by the graph.</param>
    /// <param name="action">the method sed to the edge based on the source and target transforms, respectively</param>
    void UpdateCharacter(CharacterGraph graph, GameObject soldier, Action<CharacterGraphEdge, Transform, Transform> action)
    {
        Transform[] transforms = soldier.GetComponentsInChildren<Transform>();

        foreach(CharacterGraphEdge edge in graph.Edges)
        {
            List<CharacterGraphEdge>[] outputEdges = characterMap.GetOuput(new List<CharacterGraphEdge>() { edge });

            if(outputEdges!= null)
            {
                foreach (List<CharacterGraphEdge> outputEdgeList in outputEdges)
                {
                    if (outputEdgeList != null && outputEdgeList.Count > 0)
                    {
                        CharacterGraphEdge outputEdge = outputEdgeList.First();

                        IEnumerable<Transform> vertexTransformsBegin = transforms
                                                                .Where(t => String.Compare(outputEdge.Source.Name, t.name, StringComparison.InvariantCultureIgnoreCase) == 0);
                        IEnumerable<Transform> vertexTransformsEnd = transforms
                                                         .Where(t => String.Compare(outputEdge.Target.Name, t.name, StringComparison.InvariantCultureIgnoreCase) == 0);

                        if (vertexTransformsBegin.Count() > 0 && vertexTransformsEnd.Count() > 0)
                        {
                            action.Invoke(edge, vertexTransformsBegin.First(), vertexTransformsEnd.First());
                        }

                      
                    }
                }
            }
            
           
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform">The transform to apply to each of the 3d vectors to get it in line.</param>
    /// <returns></returns>
    KeyPointMap CreateKeyPointMap(Vector3 transform)
    {
        List<MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>> entries = new List<MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>>()
         {
                new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    
                    
                     new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"nose")
                }, new CharacterGraphNode(new NodeTransform(),"head")),
                 new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"rightShoulder")
                }, new CharacterGraphNode(new NodeTransform(), "rightShoulder")),

                    new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"rightElbow")
                }, new CharacterGraphNode(new NodeTransform(),"rightElbow")),

                       new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"rightWrist")
                }, new CharacterGraphNode(new NodeTransform(),"rightWrist")),

                     new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"leftShoulder")
                }, new CharacterGraphNode(new NodeTransform(),"leftShoulder")),
                        new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"leftElbow")
                }, new CharacterGraphNode(new NodeTransform(),"leftElbow")),

                       new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"leftWrist")
                }, new CharacterGraphNode(new NodeTransform(),"leftWrist")),

                new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"rightHip")
                }, new CharacterGraphNode(new NodeTransform(),"leftHip")),

                 new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"rightKnee")
                }, new CharacterGraphNode(new NodeTransform(),"leftKnee")),

                  new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"rightAnkle")
                }, new CharacterGraphNode(new NodeTransform(),"leftAnkle")),

                   new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"leftHip")
                }, new CharacterGraphNode(new NodeTransform(),"rightHip")),

                 new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"leftKnee")
                }, new CharacterGraphNode(new NodeTransform(),"rightKnee")),

                  new MapEntry<List<PoseNet.Keypoint>, CharacterGraphNode>( new List<PoseNet.Keypoint>(){
                    new PoseNet.Keypoint(0.0f ,System.Numerics.Vector2.One,"leftAnkle")
                }, new CharacterGraphNode(new NodeTransform(),"rightAnkle"))
         };

        return new KeyPointMap(entries.ToArray(), new System.Numerics.Vector3(transform.x,transform.y,transform.z));
    }

    GraphMap CharacterGraphMap()
    {
        var entries = new List<MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>>
          {
             new MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>(

                    new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "neck"),
                         new CharacterGraphNode(new NodeTransform(), "head"))
                        },

                      new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "RigNeck"),
                         new CharacterGraphNode(new NodeTransform(), "RigHead"))
                        }),

             

              new MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>(
                    new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "leftShoulder"),
                         new CharacterGraphNode(new NodeTransform(), "leftElbow"))
                        },
                      new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "RigArmLeft1"),
                         new CharacterGraphNode(new NodeTransform(), "RigArmLeft2"))
                        }),

                  new MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>(
                    new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "leftElbow"),
                         new CharacterGraphNode(new NodeTransform(), "leftWrist"))
                        },
                      new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "RigArmLeft2"),
                         new CharacterGraphNode(new NodeTransform(), "RigArmLeft3"))
                        }),




                  new MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>(
                    new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "rightShoulder"),
                         new CharacterGraphNode(new NodeTransform(), "rightElbow"))
                        },
                      new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "RigArmRight1"),
                         new CharacterGraphNode(new NodeTransform(), "RigArmRight2"))
                        }),

                  new MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>(
                    new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "rightElbow"),
                         new CharacterGraphNode(new NodeTransform(), "rightWrist"))
                        },
                      new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "RigArmRight2"),
                         new CharacterGraphNode(new NodeTransform(), "RigArmRight3"))
                        }),

                  

                    /*new MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>(
                    new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "leftHip"),
                         new CharacterGraphNode(new NodeTransform(), "leftKnee"))
                        },
                      new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "RigLegLeft1"),
                         new CharacterGraphNode(new NodeTransform(), "RigLegLeft2"))
                        }),

                       new MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>(
                    new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "leftKnee"),
                         new CharacterGraphNode(new NodeTransform(), "leftAnkle"))
                        },
                      new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "RigLegLeft2"),
                         new CharacterGraphNode(new NodeTransform(), "RigLegLeft3"))
                        }),




                     
                    new MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>(
                    new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "rightHip"),
                         new CharacterGraphNode(new NodeTransform(), "rightKnee"))
                        },
                      new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "RigLegRight1"),
                         new CharacterGraphNode(new NodeTransform(), "RigLegRight2"))
                        }),

                       new MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>(
                    new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "rightKnee"),
                         new CharacterGraphNode(new NodeTransform(), "rightAnkle"))
                        },
                      new List<CharacterGraphEdge>(){
                        new CharacterGraphEdge(new CharacterGraphNode(new NodeTransform(), "RigLegRight2"),
                         new CharacterGraphNode(new NodeTransform(), "RigLegRight3"))
                        })*/



          };

        return new GraphMap(entries.ToArray());
    }
}
