using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using mattatz.Utils;
using mattatz.Triangulation2DSystem;

public class UsageTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // input points for a polygon2D contor
        List<Vector2> points = new List<Vector2>();

        // Add Vector2 to points
        points.Add(new Vector2(-2.5f, -2.5f));
        points.Add(new Vector2(2.5f, -2.5f));
        points.Add(new Vector2(4.5f, 2.5f));
        points.Add(new Vector2(0.5f, 4.5f));
        points.Add(new Vector2(-3.5f, 2.5f));

        // construct Polygon2D 
        Polygon2D polygon = Polygon2D.Contour(points.ToArray());

        // construct Triangulation2D with Polygon2D and threshold angle (18f ~ 27f recommended)
        Triangulation2D triangulation = new Triangulation2D(polygon, 22.5f);

        // build a mesh from triangles in a Triangulation2D instance
        Mesh mesh = triangulation.Build();

        // Initialize gameObject
        //   assuming it starts out as an empty object
        gameObject.AddComponent<MeshFilter>();

        // GetComponent<MeshFilter>().sharedMesh = mesh;
        //gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>();

        //gameObject.AddComponent<MeshCollider>();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
