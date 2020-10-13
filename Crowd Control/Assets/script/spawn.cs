using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject perso;
    public int nb_perso;

    public List<GameObject> wall_position = new List<GameObject>();
    public List<GameObject> round_position = new List<GameObject>();
    public List<GameObject> character = new List<GameObject>();


    [HideInInspector]
    public bool formation;
    // Start is called before the first frame update
    void Start()
    {
        spawn_around_me();
        take_formation();
        //no_formation();


    }
    void Update()
    {
       
    }

    public void change_formation()
    {
        if (formation)
            take_formation();
        else
            no_formation();
    }

    private void spawn_around_me()
    {
        for(int i = 0; i < nb_perso; i++)
        {
            GameObject tmp =  Instantiate(perso, transform);
            character.Add(tmp);
            GameObject.Find("CanvasTotal").GetComponent<Canvas_T>().Total_policier++;
        }
    }

    
    private void take_formation()
    {

       
        for (int i = 0; i < character.Count; i++)
        {
            character[i].GetComponent<formation>().target = wall_position[i];
            character[i].transform.LookAt(wall_position[i].transform);
           
        }
        formation = false;
    }
    public void no_formation()
    {
        
        for (int i = 0; i < character.Count; i++)
        {
            character[i].GetComponent<formation>().target = round_position[i];
            character[i].transform.LookAt(round_position[i].transform);
        }
        formation = true;
    }
}
