using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class BallTriggerObject : MonoBehaviour {

    private AudioSource aSource;
    private Animator anim;

    [SerializeField] private AudioClip triggerSound;
    [SerializeField] private AudioClip wrongTriggerSound;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
    }

    //todo
    public void BallTrigger()
    {
        Debug.Log("ball trigger " + transform.GetChild(0).name);
        anim.SetBool("SocketsActivated", true);
        aSource.PlayOneShot(triggerSound);
    }
}
