using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseProjection : MonoBehaviour
{
    private Camera cam;
    public float height = 2.0f;

    // data presented to PrismController script which generates the triggered waves
    public Vector3 locTriggered = new Vector3();
    public float timeTriggered = 0;

    void Start()
    {
        cam = Camera.main;
    }

    private void OnGUI()
    {
        Event currentEvent = Event.current;

        Vector2 mousePos2D = currentEvent.mousePosition;
        //Vector3 mousePosScreen = new Vector3(mousePos2D.x, Screen.height - mousePos2D.y, cam.nearClipPlane);
        Vector3 mousePosScreen = new Vector3(mousePos2D.x, Screen.height - mousePos2D.y, cam.farClipPlane);

        Plane xzPlane = new Plane(Vector3.up, height*Vector3.up);
        Ray ray = cam.ScreenPointToRay(mousePosScreen);
        float enter = 0.0f;

        if(xzPlane.Raycast(ray, out enter))
        {
            // Get the point of intersection
            Vector3 hit = ray.GetPoint(enter);

            gameObject.transform.position = hit - height*Vector3.up;
        }

        // if left mouse button pressed
        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
        {
            locTriggered = gameObject.transform.position;
            timeTriggered = Time.time;
        }



    }
}
