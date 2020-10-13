using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JusticeMight : MonoBehaviour
{

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("JusticeMight", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
