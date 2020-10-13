using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    //all "chief" pnj in the map
    public List<GameObject> Players;

    //Set the gameobject to display
    public GameObject Manifestant;
    public GameObject Policier;

    //refresh rate
    private float refreshRate;
    private float lastTimeRefreshed;

    //curent objects;
    private List<GameObject> currentPoints;

    private void Start()
    {
        refreshRate = 2;
        lastTimeRefreshed = 0;
        currentPoints = new List<GameObject>();
    }

    private void LateUpdate()
    {
        if (Time.fixedTime >= lastTimeRefreshed + refreshRate)
        {
            lastTimeRefreshed = Time.fixedTime;

            while (currentPoints.Count > 0)
            {
                GameObject tmp = currentPoints[0];
                currentPoints.RemoveAt(0);
                Destroy(tmp);
            }

            for (int i = 0; i < Players.Count; i++)
            {
                while (Players[i] == null)
                {
                    if (Players.Count == 0)
                        return;
                    try
                    {
                        Players.RemoveAt(i);
                        i--;
                    }
                    catch
                    {
                        return;
                    }
                }

                if (Players.Count == 0)
                    return;

                GameObject tmp = new GameObject();
                tmp.transform.position = Players[i].transform.position;
                tmp.transform.position = new Vector3(tmp.transform.position.x, 250, tmp.transform.position.z);

                tmp.name = "tempDotMap";

                if (Players[i].GetComponent<Human>().IsPoliceman)
                    Instantiate(Policier, tmp.transform);
                else
                    Instantiate(Manifestant, tmp.transform);

                currentPoints.Add(tmp);
            }
        }
        
    }
}
