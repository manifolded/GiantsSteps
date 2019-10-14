using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using mattatz.Utils;
using mattatz.Triangulation2DSystem;

public class GeneratePrism : MonoBehaviour
{
    public GameObject mesh2DHolder;
    public Transform PrismHolder;
    public Material mat;

    void Start()
    {
        Mesh mesh2D = mesh2DHolder.GetComponent<MeshFilter>().mesh;
        int[] triangles2D = mesh2D.triangles;

        int numTriangles = triangles2D.Length / 3;

        for(int frontIndex2D = 0; frontIndex2D<numTriangles; frontIndex2D++)
        {
            GenerateSinglePrism(ref mesh2D, frontIndex2D);
        }
    }

    // =========================================================================
    private void GenerateSinglePrism(ref Mesh mesh2D, int frontIndex2D)
    {
        int[] triangles2D = mesh2D.triangles;
        Vector3[] vertices2D = mesh2D.vertices;
        List<Vector3> vertices = new List<Vector3> { };
        List<int> triangles = new List<int> { };

        // select individual triangle from triangles2D with frontIndex2D
        // single triangle vertices in 2D coords
        Vector3[] triVerts2D =
        {
            vertices2D[triangles2D[frontIndex2D * 3 + 2]],
            vertices2D[triangles2D[frontIndex2D * 3 + 1]],
            vertices2D[triangles2D[frontIndex2D * 3 + 0]]
        };

        // structure required to utilize Mattatz's circumcenter code.
        List<Vertex2D> triVerts2D2 = new List<Vertex2D>();

        for (int i = 0; i < 3; i++)
        {
            Vector3 v3D = triVerts2D[i];
            Vector2 v2D = new Vector2(v3D[0], v3D[1]); // need only x & y
            triVerts2D2.Add(new Vertex2D(v2D));
        }

        Segment2D s0 = new Segment2D(triVerts2D2[0], triVerts2D2[1]);
        Segment2D s1 = new Segment2D(triVerts2D2[1], triVerts2D2[2]);
        Segment2D s2 = new Segment2D(triVerts2D2[2], triVerts2D2[0]);

        Triangle2D triangle2D2 = new Triangle2D(s0, s1, s2);
        Vector2 center2D = triangle2D2.Circumcenter();
        Vector3 center = center2D;

        // translate triangle to place circumcenter at the origin
        Vector3[] triVerts2Dshifted = new Vector3[3];
        for (int i = 0; i < 3; i++) {
            triVerts2Dshifted[i] = triVerts2D[i] - center;
        }
        AddFrontFace(vertices, triangles, triVerts2Dshifted);
        AddBackFace(vertices, triangles, triVerts2Dshifted);
        AddSideFaces(vertices, triangles, triVerts2Dshifted);

        Vector3 center3D = new Vector3(center2D[0], 0, center2D[1]);

        // Create output game object
        GameObject prism = new GameObject("Prism");
        prism.transform.parent = PrismHolder;
        prism.tag = "Prism";
        prism.transform.position = center3D;
        prism.AddComponent<MeshFilter>();
        prism.AddComponent<MeshRenderer>();
        //prism.AddComponent<FloatScalingEffect>(); // Added scaling animation
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
    private void AddFrontFace(List<Vector3> vertices, List<int> triangles,
                      Vector3[] triVerts2D)
    {
        int numVerts = vertices.Count();

        for (int i = 0; i < 3; i++)
        {
            Vector3 vertex2D = triVerts2D[i];
            // transform vertex2D to vertex3D and add
            vertices.Add(new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]));

            // add new triangle
            triangles.Add(numVerts + i);
        }
    }

    // =========================================================================
    private void AddBackFace(List<Vector3> vertices, List<int> triangles,
                     Vector3[] triVerts2D)
    {
        int numVerts = vertices.Count();

        for (int i = 0; i < 3; i++)
        {
            // pull out single vertex in 2D mesh coords
            Vector3 vertex2D = triVerts2D[(2 - i)]; // reverse orientation
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
    private void AddSideFaceFromEdge(List<Vector3> vertices, List<int> triangles,
                             Vector3[] triVerts2D, int[] edge)
    {
        int numVerts = vertices.Count();

        int ea = edge[0];
        int eb = edge[1];

        // add the 'a' vertex
        // index: numVerts + 0
        Vector3 vertex2D = triVerts2D[ea];
        // transform vertex2D to vertex3D in full 3D coords
        Vector3 vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
        // add
        vertices.Add(vertex3D);
        int a = numVerts + 0;

        // add the 'b' vertex
        // index: numVerts + 1
        vertex2D = triVerts2D[eb];
        vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
        vertices.Add(vertex3D);
        int b = numVerts + 1;

        // add the a-prime vertex | 'a' translated to back face
        // index: numVerts + 2
        vertex2D = triVerts2D[ea];
        vertex3D = new Vector3(vertex2D[0], -vertex2D[2], vertex2D[1]);
        vertex3D += new Vector3(0, 1, 0);
        vertices.Add(vertex3D);
        int ap = numVerts + 2;

        // add the b-prime vertex | 'b' translated to back face
        // index: numVerts + 3
        vertex2D = triVerts2D[eb];
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
    private void AddSideFaces(List<Vector3> vertices, List<int> triangles,
                      Vector3[] triVerts2D)
    {
        int numVerts = vertices.Count();

        // add all 3 side faces
        for (int i = 0; i < 3; i++) {
            int[] edge = { (i + 1)%3, i };

            AddSideFaceFromEdge(vertices, triangles, triVerts2D, edge);
        }

    }
}
