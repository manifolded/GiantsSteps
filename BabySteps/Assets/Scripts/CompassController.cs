using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour
{
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float phi = cam.transform.eulerAngles.y;
        transform.localRotation = Quaternion.Euler(new Vector3(90, -phi, 0));
    }
}
