using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource sound;
    private bool isStarted = false;

    void Start()
    {
        sound = gameObject.GetComponent<AudioSource>();
        sound.Play();
    }

    void Update()
    {
        if (isStarted && !sound.isPlaying)
        {
            Destroy(gameObject);
        }

        if (!isStarted && !sound.isPlaying)
        {
            sound.Play();
            isStarted = true;
        }
            
    }
}
