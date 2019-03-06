using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLRenderer : MonoBehaviour
{
    static Material lineMaterial;
    PoseNet posenet = new PoseNet();

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    public void DrawResults(PoseNet.Pose[] poses)
    {
        CreateLineMaterial();
        lineMaterial.SetPass(0);
        float minPoseConfidence = 0.15f;
       
        foreach (var pose in poses)
        {
            //DrawResults(poses);
            if (pose.score >= minPoseConfidence)
            {
               
                DrawKeypoint(pose.keypoints,
                    minPoseConfidence, 0.02f);
                //DrawSkeleton(pose.keypoints,
                   // minPoseConfidence, 0.02f);
            }
        }

       
    }

    public void DrawKeypoint(PoseNet.Keypoint[] keypoints, float minConfidence, float scale)
    {
        float radius = 0.30f;

        foreach (var keypoint in keypoints)
        {

            if (keypoint.score < minConfidence) { continue; }

            //Debug.Log(string.Format("Drawing Keypoint Score: {0} Part:{1} Location:{2}", keypoint.score, keypoint.Name, keypoint.position));

            GL.PushMatrix();
            lineMaterial.SetPass(0);
            GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.LINES);
            GL.Color(Color.red);

            for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.001f)
            {
                GL.Vertex3(
                    Mathf.Cos(theta) * radius + keypoint.position.X * scale,
                    Mathf.Sin(theta) * radius + keypoint.position.Y * scale, 0f);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
    public void DrawSkeleton(PoseNet.Keypoint[] keypoints, float minConfidence, float scale)
    {
        var adjacentKeyPoints = posenet.GetAdjacentKeyPoints(
            keypoints, minConfidence);

        foreach (var keypoint in adjacentKeyPoints)
        {
            DrawLine2D(new Vector2(keypoint.Item1.position.X * scale, keypoint.Item1.position.Y * scale),
                       new Vector2(keypoint.Item2.position.X * scale, keypoint.Item2.position.Y * scale), 0.03f);
        }
    }

    void DrawLine2D(Vector3 v0, Vector3 v1, float lineWidth)
    {
        Vector3 n = ((new Vector3(v1.y, v0.x, 0.0f)) - (new Vector3(v0.y, v1.x, 0.0f))).normalized * lineWidth;
        GL.Vertex3(v0.x - n.x, v0.y - n.y, 0.0f);
        GL.Vertex3(v0.x + n.x, v0.y + n.y, 0.0f);
        GL.Vertex3(v1.x + n.x, v1.y + n.y, 0.0f);
        GL.Vertex3(v1.x - n.x, v1.y - n.y, 0.0f);
    }
}
