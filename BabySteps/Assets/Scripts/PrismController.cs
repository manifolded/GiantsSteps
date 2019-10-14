using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismController : MonoBehaviour
{
    private Vector3 k;
    public float wavelength = 1.0f;
    private float wavenumber;
    private float v = 1;  // wave speed 1 m/sec
    private float omega;
    public float amplitude = 0.2f;

    private GameObject[] prisms;

    // Start is called before the first frame update
    void Start()
    {
        wavenumber = 2 * Mathf.PI / wavelength;
        float oneOverSqrt2 = 1 / Mathf.Sqrt(2.0f);
        k = wavenumber * new Vector3(oneOverSqrt2, 0, oneOverSqrt2);
        omega = wavenumber * v;
        prisms = GameObject.FindGameObjectsWithTag("Prism");
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject prism in prisms)
        {
            Vector3 x = prism.transform.position;
            float arg = Vector3.Dot(k, x) - omega * Time.time;
            Vector3 scale = Vector3.one;
            scale[1] += amplitude * Mathf.Cos(arg);
            prism.transform.localScale = scale;
        }
    }
}
