using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLineTrigger : MonoBehaviour {
    
    private bool triggered;
    private AudioSource aSource;


    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            aSource.Play();
            triggered = true;
        }


    }
}
