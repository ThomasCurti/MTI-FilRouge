using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMovement : MonoBehaviour
{
    //chief associated
    public GameObject Chief;

    //movement
    private bool moveUp;
    private float speed = 2;

    //reset
    private Vector3 startPosition;

    private void Start()
    {
        moveUp = true;
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Chief.GetComponent<agent>().IsSelected)
            Move();
        else if (transform.position.y != startPosition.y)
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
    }

    private void Move()
    {
        if (transform.position.y > 2)
            moveUp = false;
        else if (transform.position.y < 0)
            moveUp = true;

        if (moveUp)
            transform.Translate(0, speed * Time.deltaTime, 0);
        else
            transform.Translate(0, -speed * Time.deltaTime, 0);
    }


}
