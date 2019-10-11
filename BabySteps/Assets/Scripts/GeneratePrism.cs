﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneratePrism : MonoBehaviour
{
    public GameObject mesh2DHolder;
    public Material mat;

    void Start()
    {
        // select 2D mesh triangle to use as cross-section
        int frontIndex2D = 2;
        float depth = 20;

        Mesh mesh2D = mesh2DHolder.GetComponent<MeshFilter>().mesh;

        Vector3[] vertices2D = mesh2D.vertices;
        int[] triangles2D = mesh2D.triangles;
        List<Vector3> vertices = new List<Vector3> { };
        List<int> triangles = new List<int> { };

        AddFrontFace(vertices, triangles, vertices2D, triangles2D, frontIndex2D);
        AddBackFace(vertices, triangles, vertices2D, triangles2D, frontIndex2D, depth);
        AddSideFaces(vertices, triangles, vertices2D, triangles2D, frontIndex2D, depth);

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

        gameObject.GetComponent<MeshRenderer>().material = mat;

    }

    // =========================================================================
    void AddFrontFace(List<Vector3> vertices, List<int> triangles,
                      Vector3[] vertices2D, int[] triangles2D, int frontIndex2D)
    {
        int numVerts = vertices.Count();

        int[] triangle = {
            triangles2D[frontIndex2D * 3 + 2],
            triangles2D[frontIndex2D * 3 + 1],
            triangles2D[frontIndex2D * 3 + 0]
        };

        for (int i = 0; i < 3; i++)
        {
            Vector3 vertex2D = vertices2D[triangle[i]];
            // transform vertex2D to vertex3D and add
            vertices.Add(new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]));

            // add new triangle
            triangles.Add(numVerts + i);
        }

    }

    // =========================================================================
    void AddBackFace(List<Vector3> vertices, List<int> triangles,
                      Vector3[] vertices2D, int[] triangles2D, int frontIndex2D,
                      float depth)
    {
        int numVerts = vertices.Count();

        // reverse the orientation of the triangle
        int[] triangle = {
            triangles2D[frontIndex2D * 3 + 0],
            triangles2D[frontIndex2D * 3 + 1],
            triangles2D[frontIndex2D * 3 + 2]
        };

        for (int i = 0; i < 3; i++)
        {
            // pull out single vertex in 2D mesh coords
            Vector3 vertex2D = vertices2D[triangle[i]];
            // transform vertex2D to vertex3D in full 3D coords
            Vector3 vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
            // translate back face through depth
            vertex3D += new Vector3(0, depth, 0);
            // add
            vertices.Add(vertex3D);

            // add new triangle
            triangles.Add(numVerts + i);
        }

    }

    // =========================================================================
    void AddSideFaceFromEdge(List<Vector3> vertices, List<int> triangles,
                     Vector3[] vertices2D, int[] triangles2D,
                     int[] edge, float depth)
    {
        int numVerts = vertices.Count();

        int ea = edge[0];
        int eb = edge[1];

        // add the 'a' vertex
        // index: numVerts + 0
        Vector3 vertex2D = vertices2D[ea];
        // transform vertex2D to vertex3D in full 3D coords
        Vector3 vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
        // add
        vertices.Add(vertex3D);
        int a = numVerts + 0;

        // add the 'b' vertex
        // index: numVerts + 1
        vertex2D = vertices2D[eb];
        vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
        vertices.Add(vertex3D);
        int b = numVerts + 1;

        // add the a-prime vertex | 'a' translated to back face
        // index: numVerts + 2
        vertex2D = vertices2D[ea];
        vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
        vertex3D += new Vector3(0, depth, 0);
        vertices.Add(vertex3D);
        int ap = numVerts + 2;

        // add the b-prime vertex | 'b' translated to back face
        // index: numVerts + 3
        vertex2D = vertices2D[eb];
        vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
        vertex3D += new Vector3(0, depth, 0);
        vertices.Add(vertex3D);
        int bp = numVerts + 3;

        // add first triangle:  (a, b', a')
        triangles.Add(a);
        triangles.Add(bp);
        triangles.Add(ap);

        // add second triangle:  (a, b, b')
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(bp);
    }

    // =========================================================================
    void AddSideFaces(List<Vector3> vertices, List<int> triangles,
                      Vector3[] vertices2D, int[] triangles2D,
                      int frontIndex2D, float depth)
    {
        int numVerts = vertices.Count();

        int[] triangle =
        {
            triangles2D[frontIndex2D*3 + 0],
            triangles2D[frontIndex2D*3 + 1],
            triangles2D[frontIndex2D*3 + 2]
        };

        // add all 3 side faces
        for (int i = 0; i < 3; i++) {
            int[] edge = { triangle[i], triangle[(i + 1)%3] };

            AddSideFaceFromEdge(vertices, triangles, vertices2D, triangles2D,
                                edge, depth);
        }

    }
}




