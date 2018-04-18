using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{  
    private const int LevelArea = 10;
    private const int DragSpeed = 20;
    private const int ZoomMin = 25;
    private const int ZoomMax = 100;

    // Update is called once per frame
    void Update()
    {
        // Init camera translation for this frame.
        var translation = Vector3.zero;

        // Move camera with arrow keys
        translation += new Vector3((Input.GetAxis("Horizontal") - Input.GetAxis("Vertical")) / 2, 0,
                        (Input.GetAxis("Vertical") + Input.GetAxis("Horizontal")) / 2);

        // Move camera with mouse
        if (Input.GetMouseButton(1)) // RMB
        {
            // Hold button and drag camera around
            translation -= new Vector3(Input.GetAxis("Mouse X")*DragSpeed*Time.deltaTime - Input.GetAxis("Mouse Y")*DragSpeed*Time.deltaTime, 0,
                               Input.GetAxis("Mouse Y") *DragSpeed*Time.deltaTime + Input.GetAxis("Mouse X")*DragSpeed *Time.deltaTime);
        }

        // Keep camera within level and zoom area
        var desiredPosition = GetComponent<Camera>().transform.position + translation;
        if (desiredPosition.x < 0 || LevelArea < desiredPosition.x)
        {
            translation.x = 0;
        }
        if (desiredPosition.y < ZoomMin || ZoomMax < desiredPosition.y)
        {
            translation.y = 0;
        }
        if (desiredPosition.z < -LevelArea || 0 < desiredPosition.z)
        {
            translation.z = 0;
        }

        // Finally move camera parallel to world axis
        GetComponent<Camera>().transform.position += translation;
    }
}

