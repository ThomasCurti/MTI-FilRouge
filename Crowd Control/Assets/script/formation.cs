using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class formation : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public GameObject target;
    public bool IsSelected = false;
    public Behaviour halo;
    private GameObject save_pos_formation;

 
    // Start is called before the first frame update
    void Start()
    {
        
        halo = (Behaviour)GetComponent("Halo");
        halo.enabled = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        save_pos_formation = target;

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            target = save_pos_formation;
        navMeshAgent.SetDestination(target.transform.position);
    }
   
}
