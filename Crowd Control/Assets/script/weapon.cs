using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{

    [HideInInspector]
    public GameObject Parent;
    public GameObject EmptyParentObject;

    public Transform Target;
    public float Speed = 1;

    private float limitTime = 2;
    private Vector3 directionThrown;
    private float appearedTime;

    // Start is called before the first frame update
    void Start()
    {
        directionThrown = new Vector3(0, 0, Speed * Time.deltaTime);
        appearedTime = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.fixedTime >= appearedTime + limitTime)
        {
            Destroy(EmptyParentObject);
            Destroy(gameObject);
        }
        transform.Translate(directionThrown);
    }

    public void OnDestroy()
    {
        try
        {
            Parent.GetComponent<agent>().ThrownWeapons -= 1;
        }
        catch
        {
            //nothing
        }

        try 
        {
            Parent.GetComponent<IA_che_manif>().ThrownWeapons -= 1;
        }
        catch
        {
            //nothing
        }

        try
        {
            Destroy(EmptyParentObject);
        }
        catch
        {
            //nothing
        }



    }

}
