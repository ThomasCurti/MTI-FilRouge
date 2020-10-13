using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_manisfestant : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent navMeshAgent;

    [HideInInspector]
    public bool escape;
    // Start is called before the first frame update
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float dst = Vector3.Distance(target.transform.position, gameObject.transform.position);
        if (dst < 2  && escape)
        {
            GameObject.Find("CanvasTotal").GetComponent<Canvas_T>().nb_manif_escape++;
            Destroy(gameObject);
        }
        else
        {
            navMeshAgent.SetDestination(target.transform.position);
        }  
    }
}
