using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismController : MonoBehaviour
{
    private CompassController compassCont;
    public float amplitude = 0.2f;
    public float wavelength = 1.0f;
    public float decaylength = 3.0f;

    private float wavenumber;
    private float v = 1;  // wave speed 1 m/sec
    private float omega;

    private GameObject[] prisms;

    public GameObject target;
    private MouseProjection mouseProj;
    private Vector3 locTriggered;
    private float timeTriggered;

    // Start is called before the first frame update
    void Start()
    {

        compassCont = GameObject.Find("Compass").GetComponent<CompassController>();

        prisms = new GameObject[] { };
        //prisms = GameObject.FindGameObjectsWithTag("Prism");
        //Debug.Log(prisms.Length);

        mouseProj = target.GetComponent<MouseProjection>();
    }

    // Update is called once per frame
    void Update()
    {
        if(prisms.Length == 0)
        {
            prisms = GameObject.FindGameObjectsWithTag("Prism");
        }

        float aim = compassCont.aim;
        Vector3 dir = (Quaternion.Euler(aim * Vector3.up) * Vector3.forward).normalized;
        wavenumber = 2 * Mathf.PI / wavelength;
        omega = wavenumber * v;
        Vector3 k = wavenumber * dir;

        // get event data from MouseProjection
        locTriggered = mouseProj.locTriggered;
        timeTriggered = mouseProj.timeTriggered;

        foreach (GameObject prism in prisms)
        {
            Vector3 x = prism.transform.position;

            // function:  cos(k.x - w t)
            //float phase = Vector3.Dot(k, x) - omega * Time.time;
            //float function = Mathf.Cos(phase);

            // function: (1/r) Sech(j r*) Sin(k r*)
            // where r* = r - v t in the horizontal plane
            Vector3 xOffset = x - locTriggered;
            float timeOffset = Time.time - timeTriggered;

            float rSq = xOffset[0] * xOffset[0] + xOffset[2] * xOffset[2];
            float rStar = Mathf.Sqrt(rSq) - v * Time.time;
            float decaynumber = 2.0f * Mathf.PI / decaylength;
            float function = 0;
            if(timeOffset > 0)
            {
                function = Mathf.Sin(wavenumber * rStar) / (Mathf.Sqrt(rSq) * Cosh(decaynumber * rStar));
            }

            // scale prism with profile 'function'
            Vector3 scale = Vector3.one;
            scale[1] += amplitude * function;
            prism.transform.localScale = scale;
        }
    }

    private static float Cosh(float x)
    {
        return (float)System.Math.Cosh((double)x);
    }


}
