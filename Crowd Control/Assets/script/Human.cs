using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour
{
    public bool IsPoliceman = false;
    public bool IsChief = false;

    public GameObject particle;
    public GameObject sound;

    private Animator myAnimator;

    // chance de hop de départ
    private int hopChance = 0;

    // vitesse à laquelle la chance de hop augmente
    private int hopRateIncrease;

    // Booléen servant à gérer les points de vie (manifestant only)
    private bool isFullLife = true;


    private bool kak_activated = false;
    void Start()
    {

        myAnimator = gameObject.GetComponent<Animator>();

        // random car certains personnage sont plus excités que d'autres
        hopRateIncrease = Random.Range(3, 15);

        InvokeRepeating("decideHopping", Random.Range(0.4f, 0.8f), 0.35f);
    }

    void Update()
    {
        AttackKak();
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsPoliceman)
        {

            //if hit by something else than PoliceWeapon, leave
            if (collision.collider.gameObject.layer != 13)
                return;

            //quand le manifestant prend un coup de matraque volante en ayant tous ses hp
            if (collision.collider.gameObject.name != "bullet" && isFullLife)
            {
                isFullLife = false;
                return;
            }

            //if manifestant and hit by PoliceWeapon
            if (!IsChief)
            {
                KillManifestant(collision);
            }
            else
            {
                KillManifestantChief(collision);
            }
        }
        else //ispoliceman
        {
            //if hit by something else than ManifestantWeapon, leave
            if (collision.collider.gameObject.layer != 14)
                return;

            if (!IsChief)
            {
                KillPoliceman(collision);
            }
            else
            {
                KillPolicemanChief(collision);
            }
           
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == 14)
            return;
        if (gameObject.layer == 11)
        {
            if (other.gameObject.layer == 11)
                return;
            GameObject res = ChooseTargetKak(other.gameObject);
            if ( res != null)
            {
                gameObject.GetComponent<formation>().target = res;
                kak_activated = true;
                StartCoroutine(AttackKak());
            }
            
                
        }
    }


    private void KillManifestant(Collision collision)
    {
        //instantiate blood and sound
        GameObject tmp = new GameObject();
        tmp.transform.position = transform.position;
        Instantiate(particle, tmp.transform.position, tmp.transform.rotation);
        Instantiate(sound, tmp.transform.position, tmp.transform.rotation);
        tmp.name = "tempKillManifestant";

        //escape
        if(gameObject.GetComponentInParent<IA_che_manif>() != null)
            gameObject.GetComponentInParent<IA_che_manif>().escape = true;

        //death animation
        myAnimator.Play("manifdeath");

        //destroy
        StartCoroutine(destroyAfterTime(gameObject, 0.7f));
        Destroy(tmp);
        Destroy(collision.gameObject);
    }

    private void KillManifestantChief(Collision collision)
    {
        //get list of underlings
        List<GameObject> underlings = new List<GameObject>();
        try
        {
            //if manifestant is dead => null
            underlings = GetComponent<IA_che_manif>().character;
        }
        catch
        {
            //nothing to do
        }
        //if any underlings still lives, stay alive.
        for (int i = 0; i < underlings.Count; i++)
        {
            if (underlings[i] != null)
                return;
        }
        
        //Otherwise, die too.
        myAnimator.Play("manifdeath");
        StartCoroutine(destroyAfterTime(gameObject, 0.7f));
        Destroy(collision.gameObject);
    }


    private void KillPoliceman(Collision collision)
    {
        //instantiate blood and sound
        GameObject tmp = new GameObject();
        tmp.transform.position = transform.position;
        tmp.name = "tempKillPoliceman";
        Instantiate(particle, tmp.transform.position, tmp.transform.rotation);
        Instantiate(sound, tmp.transform.position, tmp.transform.rotation);

        //destroy everything
        myAnimator.Play("policedeath");
        StartCoroutine(destroyAfterTime(gameObject, 0.7f));
        Destroy(tmp);
        Destroy(collision.gameObject);
    }

    private void KillPolicemanChief(Collision collision)
    {
        //get list of underlings
        List<GameObject> underlings = new List<GameObject>();
        try
        {
            //if manifestant is dead => null
            underlings = GetComponent<agent>().agents;
        }
        catch
        {
            //nothing to do
        }
        //if any underlings still lives, stay alive.
        for (int i = 0; i < underlings.Count; i++)
        {
            if (underlings[i] != null)
                return;
        }
        //Otherwise, die too.
        myAnimator.Play("policedeath");
        StartCoroutine(destroyAfterTime(gameObject, 0.7f));
        Destroy(collision.gameObject);
    }

   private GameObject ChooseTargetKak(GameObject g_colision) //renvoie une cible pour le corp a corp
    {
        List<GameObject> target_list = null;

        if(gameObject.layer == 11 || gameObject.layer == 12)
        {
            if (g_colision.layer == 10)
            {
                if (g_colision.tag == "manifestant_chef")
                    target_list = g_colision.GetComponent<IA_che_manif>().character;
                else
                    target_list = g_colision.GetComponentInParent<IA_che_manif>().character;
            }
        }
       /* if (gameObject.layer == 10) // pour implem fight manifestant
        {
            if (g_colision.layer == 11)
            {
                target_list = g_colision.GetComponentInParent<spawn>().character;
            }
            if (g_colision.layer == 12)
            {
                target_list = g_colision.GetComponent<spawn>().character;
            }
        }*/
        if (target_list == null)
            return null;
        GameObject tmp_target = null;
        
        for (int i = 0; i < target_list.Count; i++)
        {
            if (target_list[i] == null)
                continue;
            if(Vector3.Distance(target_list[i].transform.position, gameObject.transform.position) < 5)
            {
                if (tmp_target == null)
                    tmp_target = target_list[i];
               if(Vector3.Distance(tmp_target.transform.position, gameObject.transform.position) > Vector3.Distance(target_list[i].transform.position, gameObject.transform.position))
                    tmp_target = target_list[i];
            }
        }
        return tmp_target;
    }

    private IEnumerator AttackKak()
    {
        if((gameObject.layer == 11 || gameObject.layer == 12 ) && kak_activated)
        {
           GameObject tmp =  gameObject.GetComponent<formation>().target;
            if (tmp.GetComponent<Human>() != null)
            {
                if (Vector3.Distance(tmp.transform.position, gameObject.transform.position) <= 2)
                {
                    
                    if (tmp.GetComponent<Human>().isFullLife)
                        isFullLife = false;
                    else
                    {                      
                        if (!tmp.GetComponent<Human>().IsChief)
                        {
                            death_kak(tmp);
                        }
                        else
                        {
                            bool ok_to_kill = false;
                            List<GameObject> underlings = new List<GameObject>();
                            try
                            {
                                underlings = GetComponent<IA_che_manif>().character;
                            }
                            catch
                            {
                            }
                            for (int i = 0; i < underlings.Count; i++)
                            {
                                if (underlings[i] != null)
                                    ok_to_kill = false;
                            }
                            if(ok_to_kill)
                                death_kak(tmp);
                        }
                        kak_activated = false;
                    }

                }
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(AttackKak());
        }
    }

    private void death_kak(GameObject dead)
    {
        GameObject tmp = new GameObject();
        tmp.transform.position = transform.position;
        Instantiate(particle, tmp.transform.position, tmp.transform.rotation);
        Instantiate(sound, tmp.transform.position, tmp.transform.rotation);
        tmp.name = "tempKillManifestant";

        //death animation
        myAnimator.Play("manifdeath");

        //destroy
        Destroy(dead);
    }

    // Effectue des checks pour savoir si le personnage doit demarrer
    // ou arrêter l'animation de saut.
    // Doit être lancée à une période t < 0.4f (durée de l'animation)
    private void decideHopping()
    {
        bool isHopping = myAnimator.GetBool("isHopping");
        if (isHopping)
            myAnimator.SetBool("isHopping", false);
        else
        {
            if (hopChance >= Random.Range(1, 100))
            {
                myAnimator.SetBool("isHopping", true);
                hopChance = 0;
            }
            else
                hopChance += hopRateIncrease;
        }

    }

    IEnumerator destroyAfterTime(GameObject go, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

}
