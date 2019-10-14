using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using mattatz.Utils;
using mattatz.Triangulation2DSystem;

using System.Linq;

public class UsageTest : MonoBehaviour
{
    void Awake()
    {
        // input points for a polygon2D contour
        List<Vector2> points = new List<Vector2>();
        // When calling PopulateSquarePerimeter you should prefer
        // a factor of 4 for the 'res' argument.
        PopulateSquarePerimeter(points, 1000, 50);

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
        // gameObject.AddComponent<MeshRenderer>();

        //gameObject.AddComponent<MeshCollider>();

        gameObject.SetActive(false);
    }

    // =========================================================================
    private List<int> PopulateInterval(int res, int num)
    {
        List<int> vals = new List<int>();

        for (int i=0; i<num; i++)
            vals.Add(Random.Range(0, res));

        // make sure the four corners will be represented
        for(int i=0; i<4; i++)
            vals.Add(i * res/4);

        // sort - points on perimeter must be ordered
        vals.Sort();

        // return with duplicates removed
        return vals.Distinct().ToList();
    }

    // =========================================================================
    private void PopulateSquarePerimeter(List<Vector2> points, int res, int num) 
    {
        List<int> vals = PopulateInterval(res, num);

        if (res < 4)
            Debug.Log("resolution, res, must be 4 or larger.");
        if (num < 4)
            Debug.Log("num vertices, num, must be 4 or larger.");

        for (int i=0; i<vals.Count; i++)
        {
            // faces of the square indexed as 0, 1, 2, 3
            int q = 4 * vals[i] / res; // integer division intended
            float s = 8.0f * vals[i] / res;  // [0, 8)

            int qm2 = q % 2;  // quadrant mod 2
            int nqm2 = -qm2 + 1;  // complement of quadrant mod 2
            int qo2 = q / 2;  // integer division intended

            int sign = -2 * qo2 + 1;
            int offset = -(2 * q + 1);

            float sq = (s + offset)*sign;

            // map the randomly covered interval to the square
            //  using integer black magic
            float x = sign * qm2 + nqm2 * sq;
            float y = sign * -nqm2 + qm2 * sq;

            points.Add(new Vector2(x, y));
        }
    }
    

}
