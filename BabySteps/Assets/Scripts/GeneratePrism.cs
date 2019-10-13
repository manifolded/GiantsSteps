using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneratePrism : MonoBehaviour
{
    public GameObject mesh2DHolder;
    public Transform PrismHolder;
    public Material mat;

    void Start()
    {
        //float depth = 20.0f;
        Mesh mesh2D = mesh2DHolder.GetComponent<MeshFilter>().mesh;
        int[] triangles2D = mesh2D.triangles;

        int numTriangles = triangles2D.Length / 3;

        for(int frontIndex2D = 0; frontIndex2D<numTriangles; frontIndex2D++)
        {
            GenerateSinglePrism(ref mesh2D, frontIndex2D);
        }

    }

    // =========================================================================
    void GenerateSinglePrism(ref Mesh mesh2D, int frontIndex2D)
    {

        Vector3[] vertices2D = mesh2D.vertices;
        int[] triangles2D = mesh2D.triangles;
        List<Vector3> vertices = new List<Vector3> { };
        List<int> triangles = new List<int> { };

        AddFrontFace(vertices, triangles, ref mesh2D, frontIndex2D);
        AddBackFace(vertices, triangles, ref mesh2D, frontIndex2D);
        AddSideFaces(vertices, triangles, ref mesh2D, frontIndex2D);

        // Create output game object
        GameObject prism = new GameObject("Prism");
        prism.transform.parent = PrismHolder;
        prism.transform.localScale += Vector3.up * Random.Range(1.0f, 2.0f);
        prism.AddComponent<MeshFilter>();
        prism.AddComponent<MeshRenderer>();
        prism.AddComponent<FloatScalingEffect>(); // Added scaling animation
        Mesh mesh = prism.GetComponent<MeshFilter>().mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        //mesh.uv = new Vector2[]
        //    { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        prism.GetComponent<MeshRenderer>().material = mat;

    }

    // =========================================================================
    void AddFrontFace(List<Vector3> vertices, List<int> triangles,
                      ref Mesh mesh2D, int frontIndex2D)
    {
        Vector3[] vertices2D = mesh2D.vertices;
        int[] triangles2D = mesh2D.triangles;

        int numVerts = vertices.Count();

        int[] triangle2D = {
            triangles2D[frontIndex2D * 3 + 2],
            triangles2D[frontIndex2D * 3 + 1],
            triangles2D[frontIndex2D * 3 + 0]
        };

        for (int i = 0; i < 3; i++)
        {
            Vector3 vertex2D = vertices2D[triangle2D[i]];
            // transform vertex2D to vertex3D and add
            vertices.Add(new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]));

            // add new triangle
            triangles.Add(numVerts + i);
        }

    }

    // =========================================================================
    void AddBackFace(List<Vector3> vertices, List<int> triangles,
                     ref Mesh mesh2D, int frontIndex2D)
    {
        Vector3[] vertices2D = mesh2D.vertices;
        int[] triangles2D = mesh2D.triangles;

        int numVerts = vertices.Count();

        // reverse the orientation of the triangle
        int[] triangle2D = {
            triangles2D[frontIndex2D * 3 + 0],
            triangles2D[frontIndex2D * 3 + 1],
            triangles2D[frontIndex2D * 3 + 2]
        };

        for (int i = 0; i < 3; i++)
        {
            // pull out single vertex in 2D mesh coords
            Vector3 vertex2D = vertices2D[triangle2D[i]];
            // transform vertex2D to vertex3D in full 3D coords
            Vector3 vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
            // translate back face through depth
            vertex3D += new Vector3(0, 1, 0);
            // add
            vertices.Add(vertex3D);

            // add new triangle
            triangles.Add(numVerts + i);
        }

    }

    // =========================================================================
    void AddSideFaceFromEdge(List<Vector3> vertices, List<int> triangles,
                             ref Mesh mesh2D, int[] edge)
    {
        Vector3[] vertices2D = mesh2D.vertices;
        int[] triangles2D = mesh2D.triangles;

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
        vertex3D += new Vector3(0, 1, 0);
        vertices.Add(vertex3D);
        int ap = numVerts + 2;

        // add the b-prime vertex | 'b' translated to back face
        // index: numVerts + 3
        vertex2D = vertices2D[eb];
        vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
        vertex3D += new Vector3(0, 1, 0);
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
                      ref Mesh mesh2D, int frontIndex2D)
    {
        Vector3[] vertices2D = mesh2D.vertices;
        int[] triangles2D = mesh2D.triangles;

        int numVerts = vertices.Count();

        int[] triangle2D =
        {
            triangles2D[frontIndex2D*3 + 0],
            triangles2D[frontIndex2D*3 + 1],
            triangles2D[frontIndex2D*3 + 2]
        };

        // add all 3 side faces
        for (int i = 0; i < 3; i++) {
            int[] edge = { triangle2D[i], triangle2D[(i + 1)%3] };

            AddSideFaceFromEdge(vertices, triangles, ref mesh2D, edge);
        }

    }
}




