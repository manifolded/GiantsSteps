using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneratePrism : MonoBehaviour
{
    public GameObject mesh2DHolder;
    public Material mat;

    void Start()
    {
        Mesh mesh2D = mesh2DHolder.GetComponent<MeshFilter>().mesh;

        Vector3[] vertices2D = mesh2D.vertices;
        int[] triangles2D = mesh2D.triangles;

        // select triangle from 2D mesh
        int triangle2DIndex = 0;
        int[] triangle2D =
            {triangles2D[triangle2DIndex*3 + 0],
             triangles2D[triangle2DIndex*3 + 1],
             triangles2D[triangle2DIndex*3 + 2]
            };

        List<int> triangles = new List<int> { 0, 1, 2 };
        //List<Vector3> vertices = new List<Vector3>
        //{
        //    vertices2D[triangle2D[0]],
        //    vertices2D[triangle2D[1]],
        //    vertices2D[triangle2D[2]]
        //};

        List<Vector3> vertices = new List<Vector3> {};

        for (int i = 0; i < 3; i++) {

            Vector3 xyVertex = vertices2D[triangle2D[i]];

            // perform coordinate rotation as vertices are added.
            vertices.Add(new Vector3(xyVertex[0], -xyVertex[2], xyVertex[1]));

        }

        //Debug.Log(vertices[0]);
        //Debug.Log(vertices[1]);
        //Debug.Log(vertices[2]);

        // add the back face
        Vector3 depth = new Vector3(0, 20, 0);
        vertices.Add(vertices[0] + depth);
        vertices.Add(vertices[1] + depth);
        vertices.Add(vertices[2] + depth);

        //   note order intentionally reversed for outward pointing normal
        triangles.Add(5);
        triangles.Add(4);
        triangles.Add(3);

        // add side face for (0,1) edge
        //    1st triangle (0, 3, 4)
        triangles.Add(0);
        triangles.Add(3);
        triangles.Add(4);
        //    2nd triangle (0, 4, 1)
        triangles.Add(0);
        triangles.Add(4);
        triangles.Add(1);

        // add side face for (1, 2) edge
        //    1st triangle (1, 4, 5)
        triangles.Add(1);
        triangles.Add(4);
        triangles.Add(5);
        //    2nd triangle (1, 5, 2)
        triangles.Add(1);
        triangles.Add(5);
        triangles.Add(2);

        // add side face for (2, 0) edge
        //    1st triangle (2, 5, 3)
        triangles.Add(2);
        triangles.Add(5);
        triangles.Add(3);
        //    2nd triangle (2, 3, 0)
        triangles.Add(2);
        triangles.Add(3);
        triangles.Add(0);


        // Assemble output game object
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        //mesh.uv = new Vector2[]
        //    { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        //mesh.Optimize();


        gameObject.GetComponent<MeshRenderer>().material = mat;

    }

}
