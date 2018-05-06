using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class VoiceLineTrigger : MonoBehaviour {
    
    //pitäsköhän nää tehdä listalla tai jtn?
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private float waitBetweenAudio;

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
            foreach (VoiceLineTrigger vlc in sameVoiceLine) vlc.Triggered = true;
            StartCoroutine(PlayClips());
        }
    }

    IEnumerator PlayClips()
    {
        aSource.PlayOneShot(clip1);
        yield return new WaitForSeconds(waitBetweenAudio + clip1.length);
        if (clip2 != null) aSource.PlayOneShot(clip2);
    }
      
}
