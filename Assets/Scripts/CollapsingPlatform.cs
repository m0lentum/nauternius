using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

    //todo timereita pois, muuttujien yms. siistiminen
public class CollapsingPlatform : MonoBehaviour {

    [SerializeField] private float animationWaitTime;
    [SerializeField] private float resetTime;

    private GameObject platform;
    private Renderer rend;
    private Collider coll;
    private Animator anim;
    private float dropTimer;
    private float resetTimer;
    private bool timerStarted;
    private Vector3 startingPos;
    private AudioSource aSource;

	private void Awake ()
    {
        platform = transform.parent.gameObject;
        startingPos = platform.transform.position;
        rend = platform.GetComponent<Renderer>();
        coll = platform.GetComponent<Collider>();
        anim = platform.GetComponent<Animator>();
        aSource = platform.GetComponent<AudioSource>();
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
        Debug.Log("dropplat kutsuttu");
        aSource.Play();
        anim.SetBool("DropTriggered", true);
    }

    public void ResetPlatform()
    {
        Debug.Log("reset kutsuttu");
        anim.SetBool("DropTriggered", false);
        timerStarted = false;
        dropTimer = 0f;
        resetTimer = 0f;
    }

}
