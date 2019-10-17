using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour
{
    private Camera cam;

    public float aim = 0;
    public float aimSpeed = 25;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float aimAxis = Input.GetAxis("Aim");
        aim += aimAxis * Time.deltaTime * aimSpeed;

        float phi = cam.transform.eulerAngles.y;
        transform.localRotation = Quaternion.Euler(new Vector3(90, aim - phi, 0));
    }
}
