using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class BallTriggerObject : MonoBehaviour {

    private AudioSource aSource;

    [SerializeField] private AudioClip triggerSound;
    [SerializeField] private AudioClip wrongTriggerSound;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>(); 
    }

    //todo
    public void BallTrigger()
    {
        Debug.Log("ball trigger " + transform.GetChild(0).name);
        transform.GetChild(0).position = transform.GetChild(0).position - new Vector3(0,200, 0);
        aSource.PlayOneShot(triggerSound);
    }
}
