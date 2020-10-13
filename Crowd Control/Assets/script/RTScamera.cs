using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTScamera : MonoBehaviour
{
    //Corner
    public Transform TopLeftCorner;
    public Transform BackRightCorner;

    //Real camera
    public Transform Camera;

    //Start position of Camera
    private Vector3 startCameraPosition;
    private float speedMultiplier = 2f;

    void Start()
    {
        transform.position = Camera.position;
        startCameraPosition = Camera.transform.position;
    }

    void Update()
    {

        //Zoom camera
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.transform.position.y < (startCameraPosition.y + 100)) // back limit -100
        {
            Camera.transform.Translate(0, 0, -5);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.transform.position.y > (startCameraPosition.y - 100)) // forward limit 48
        {
            Camera.transform.Translate(0, 0, 5);
        }

        //Move Faster or not
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            speedMultiplier = 4f;
        else
            speedMultiplier = 2f;

        //Move camera
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0 && transform.position.x >= BackRightCorner.position.x)
            horizontal = 0;
        if (horizontal < 0 && transform.position.x <= TopLeftCorner.position.x)
            horizontal = 0;
        
        if (vertical > 0 && transform.position.z >= TopLeftCorner.position.z)
            vertical = 0;
        if (vertical < 0 && transform.position.z <= BackRightCorner.position.z)
            vertical = 0;



        transform.Translate(horizontal * speedMultiplier, 0, vertical * speedMultiplier);

    }
}
