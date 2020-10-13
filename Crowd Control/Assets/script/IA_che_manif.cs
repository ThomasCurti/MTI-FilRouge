using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_che_manif : MonoBehaviour
{
    public GameObject perso1;
    public GameObject perso2;
    public int nb_perso;
    public List<GameObject> character = new List<GameObject>();
    public List<GameObject> formation = new List<GameObject>();

    private NavMeshAgent navMeshAgent;
    public List<GameObject> checkpoint = new List<GameObject>();
    private int pos_checkpoint;
    private GameObject Target;

    public List<GameObject> escape_point = new List<GameObject>();

    //throwing object /!\ if ViolenceScale == Weapons.Count - 1 => lethal mode
    public List<GameObject> Weapons;
    public int ViolenceScale = 0;

    //prevent spamming
    public int ThrownWeapons = 0;
    public int LimitThrownWeapons = 2;

    private float lastThrow = 0;
    private float counterBeforeNextThrow = 6;

    public bool escape;

    private bool already_escape;
    void Start()
    {
        GameObject.Find("CanvasTotal").GetComponent<Canvas_T>().Total_manif += 1 + nb_perso;
        spawn_around_me();
        take_formation();
        navMeshAgent = GetComponent<NavMeshAgent>();
        pos_checkpoint = 0;
        Target = checkpoint[pos_checkpoint];


    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();

        if (escape && !already_escape)
        {
            
            escape_my_minions();
            already_escape = true;

        }
        else
        {
            if (Target == null)
                Target = checkpoint[pos_checkpoint];
            follow_my_path();
        }

        float dst = Vector3.Distance(Target.transform.position, gameObject.transform.position);
        if (dst < 2 && escape)
        {
            GameObject.Find("CanvasTotal").GetComponent<Canvas_T>().nb_manif_escape++;
            Destroy(gameObject);
        }
    }


    private void spawn_around_me()
    {
        for (int i = 0; i < nb_perso; i++)
        {
            int alea = Random.Range(0, 10);
            GameObject tmp;
            if (alea < 5)
            {
                tmp = Instantiate(perso1, transform);
            }
            else
            {
                tmp = Instantiate(perso2, transform);
            }
            character.Add(tmp);
        }
    }

    private void take_formation()
    {
        for (int i = 0; i < character.Count; i++)
        {
            character[i].GetComponent<IA_manisfestant>().target = formation[i];
            character[i].transform.LookAt(formation[i].transform);

        }
    }

    private void follow_my_path() // passe par des checkpoint
    {


        float dst = Vector3.Distance(transform.position, Target.transform.position);
        if (dst <= 3)
        {
            if (pos_checkpoint < checkpoint.Count)
            {
                Target = checkpoint[pos_checkpoint];
                pos_checkpoint++;
            }
        }
        navMeshAgent.SetDestination(Target.transform.position);
    }

    private void escape_my_minions() // faire fuir le groupe
    {
        int len = escape_point.Count;
        int alea;
        for (int i = 0; i < character.Count; i++)
        {
            if(character[i] != null)
            {
                alea = Random.Range(0, len);
                character[i].GetComponent<IA_manisfestant>().target = escape_point[alea];
                character[i].transform.LookAt(escape_point[alea].transform);
                character[i].GetComponent<IA_manisfestant>().escape = true; 
            }
            

        }
        alea = Random.Range(0, len);
        Target = escape_point[alea];
          
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
        if (other.gameObject.layer != 11 && other.gameObject.layer != 12)
            return;

        if (ThrownWeapons >= LimitThrownWeapons)
            return;

        if (Time.fixedTime < lastThrow + counterBeforeNextThrow)
            return;

        ThrownWeapons++;
        lastThrow = Time.fixedTime;


        //get the target
        Transform target = other.transform;

        //Create new transform
        GameObject tmpObject = new GameObject();
        var index = getRandomManifestant();
        if (index == -1)
            tmpObject.transform.position = transform.position;
        else
            tmpObject.transform.position = character[index].transform.position;
        tmpObject.transform.LookAt(target);

        tmpObject.name = "tempBench";

        //instantiate a new weapon
        var obj = Instantiate(Weapons[ViolenceScale], tmpObject.transform);
        obj.GetComponent<weapon>().Target = target;
        obj.GetComponent<weapon>().Speed = 10;
        obj.GetComponent<weapon>().Parent = gameObject;
        obj.GetComponent<weapon>().EmptyParentObject = tmpObject;
    }

    private int getRandomManifestant()
    {
        System.Random rand = new System.Random();
        var index = rand.Next(0, character.Count);

        while (character[index] == null && character.Count != 0)
        {
            character.RemoveAt(index);
            index = rand.Next(0, character.Count);
        }

        if (character.Count == 0)
            return -1;
        return index;
    }
}
