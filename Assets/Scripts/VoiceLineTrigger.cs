using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class VoiceLineTrigger : MonoBehaviour {
    
    public bool Triggered { get; set; }
    private AudioSource aSource;
    private List<VoiceLineTrigger> sameVoiceLine = new List<VoiceLineTrigger>();
    
    private void Start()
    {
        aSource = GetComponent<AudioSource>();

        //Lisää listaan kaikki lapsi-skriptit eli skriptit, joissa sama voiceline eri triggerboxissa
        if (transform.parent != null)
        {
            foreach (VoiceLineTrigger vcl in transform.parent.GetComponentsInChildren<VoiceLineTrigger>()) sameVoiceLine.Add(vcl);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Triggered)
        {
            aSource.Play();
            foreach (VoiceLineTrigger vlc in sameVoiceLine) vlc.Triggered = true;
        }
    }
}
