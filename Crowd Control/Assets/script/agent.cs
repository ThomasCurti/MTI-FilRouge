using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;
using System;

public class agent : MonoBehaviour
{
    //True if player selected this agent
    public bool IsSelected = false;

    //Other agents
    public List<GameObject> agents;

    //Halo
    private Behaviour halo;

    //Navigation Mesh of the chief
    private NavMeshAgent navMeshAgent;

    //throwing object /!\ if ViolenceScale == Weapons.Count - 1 => lethal mode
    public List<GameObject> Weapons;
    public static int ViolenceScale = 0;

    //prevent spamming
    public int ThrownWeapons = 0;
    public int LimitThrownWeapons = 5;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        halo = (Behaviour)GetComponent("Halo");
        halo.enabled = false;
        agents = GetComponent<spawn>().character;
    }

    void Update()
    {
        

        //If selected and right click
        if (IsSelected && Input.GetMouseButtonDown(1))
            MoveToClick();

        //If selected and left click
        if (Input.GetMouseButtonDown(0))
            Select();

    }

    //Move to the position where your mouse point
    private void MoveToClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            navMeshAgent.SetDestination(hit.point);
        }
            
    }

    //Select the unit near your mouse position
    private void Select()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool val = false;
        if (Physics.Raycast(ray, out hit))
            val = Vector3.Distance(transform.position, hit.point) < 10;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (val)
                IsSelected = val;
        }
        else
        {
            IsSelected = val;
        }

        ExpandAction(IsSelected);

        if (IsSelected)
            halo.enabled = true;
        else
            halo.enabled = false;
    }


    //Set IsSelected and halo to true/false to other agents
    private void ExpandAction(bool value)
    {
        ///!\ don't make foreach because of null
        for (int i = 0; i < agents.Count; i++)
        {
            if (agents[i] == null)
                continue;

            agents[i].GetComponent<formation>().IsSelected = value;
            agents[i].GetComponent<formation>().halo.enabled = value;

        }
    }

    void OnTriggerStay(Collider other)
    {
        Triggered(other);
    }

    public void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }

    private void Triggered(Collider other)
    {

        //10 = Manifestant
        if (other.gameObject.layer != 10)
            return;

        if (ThrownWeapons >= LimitThrownWeapons)
            return;

        ThrownWeapons++;

        //get the target
        Transform target = other.transform;

        //Create new transform
        GameObject tmpObject = new GameObject();
        Transform tmp = tmpObject.transform;
        var index = getRandomAgent();
        if (index == -1)
            tmp.position = transform.position;
        else
            tmp.position = agents[index].transform.position;
        tmp.LookAt(target);

        tmp.name = "tempStickOfJustice";

        //instantiate a new weapon
        var obj = Instantiate(Weapons[ViolenceScale], tmp.transform);
        obj.GetComponent<weapon>().Target = target;
        obj.GetComponent<weapon>().Speed = 10;
        //if bullet
        if (ViolenceScale == 1)
            obj.GetComponent<weapon>().Speed = 20;
        obj.GetComponent<weapon>().Parent = gameObject;
        obj.GetComponent<weapon>().EmptyParentObject = tmpObject;
    }

    private int getRandomAgent()
    {
        System.Random rand = new System.Random();
        var index = rand.Next(0, agents.Count);
        
        while (agents[index] == null && agents.Count != 0)
        {
            agents.RemoveAt(index);
            index = rand.Next(0, agents.Count);
        }

        if (agents.Count == 0)
            return -1;
        return index;
    }


}
