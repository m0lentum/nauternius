using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class VoiceLineTrigger : MonoBehaviour {
    
    //pitäsköhän nää tehdä listalla tai jtn?
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private float waitBetweenAudio;
    [SerializeField] string textLine;
    [SerializeField] Text voiceLineText;

    public bool Triggered { get; set; }
    private AudioSource aSource;
    private CanvasGroup canvasGroup;
    private List<VoiceLineTrigger> sameVoiceLine = new List<VoiceLineTrigger>();
    
    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        canvasGroup = voiceLineText.transform.parent.GetComponent<CanvasGroup>();

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
            StartCoroutine(TypeText(0));
        }
    }

    IEnumerator PlayClips()
    {
        aSource.PlayOneShot(clip1);
        yield return new WaitForSeconds(waitBetweenAudio + clip1.length);
        if (clip2 != null) aSource.PlayOneShot(clip2);
    }

    //Kirjoittaa tekstiä kirjain kerrallaan näytölle
    IEnumerator TypeText(int startIndex)
    {
        canvasGroup.alpha = 1f;
        voiceLineText.text = "";
        for (int i = startIndex; i < textLine.Length; i++)
        {
            char letter = textLine.ToCharArray()[i];
            voiceLineText.text += letter;
            if (letter == ' ' && voiceLineText.text.Length > 50)
            {
                StartCoroutine(TypeText(i));
                break;
            }
            if (i == textLine.Length -1) StartCoroutine(FadeCanvas());
            yield return new WaitForSeconds(0.1f);
        }
    }

    //Odottaa vähän ja alkaa feidaamaan canvasta
    public IEnumerator FadeCanvas()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i == 0) yield return new WaitForSeconds(3);
            canvasGroup.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

}
