using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class CollapsingPlatform : MonoBehaviour {

    [SerializeField] private float animationWaitTime;
    [SerializeField] private float resetTime;
    
    private AudioSource aSource;
    private Animator anim;
    private float dropTimer;
    private float resetTimer;
    private bool timerStarted;

	private void Awake ()
    {
        anim = transform.parent.gameObject.GetComponent<Animator>();
        aSource = transform.parent.gameObject.GetComponent<AudioSource>();
	}
	
	void Update ()
    {
        if (timerStarted)
        {
            dropTimer += Time.deltaTime;
            resetTimer += Time.deltaTime;

            if(resetTimer > animationWaitTime + resetTime)
            {
                ResetPlatform();
            }
            else if (dropTimer > animationWaitTime)
            {
                DropPlatform();
                dropTimer = -resetTime * 2;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timerStarted = true;
        }
    }

    void DropPlatform()
    {
        aSource.Play();
        anim.SetBool("DropTriggered", true);
    }

    public void ResetPlatform()
    {
        anim.SetBool("DropTriggered", false);
        timerStarted = false;
        dropTimer = 0f;
        resetTimer = 0f;
    }

}
