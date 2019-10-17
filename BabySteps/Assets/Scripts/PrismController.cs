using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismController : MonoBehaviour
{
    private CompassController compassCont;
    public float wavelength = 1.0f;
    private float wavenumber;
    private float v = 1;  // wave speed 1 m/sec
    private float omega;
    public float amplitude = 0.2f;

    private GameObject[] prisms;

    // Start is called before the first frame update
    void Start()
    {

        compassCont = GameObject.Find("Compass").GetComponent<CompassController>();

        prisms = new GameObject[] { };
        //prisms = GameObject.FindGameObjectsWithTag("Prism");
        //Debug.Log(prisms.Length);
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

        foreach (GameObject prism in prisms)
        {
            Vector3 x = prism.transform.position;
            float phase = Vector3.Dot(k, x) - omega * Time.time;
            Vector3 scale = Vector3.one;
            scale[1] += amplitude * Mathf.Cos(phase);
            prism.transform.localScale = scale;
        }
    }
}
