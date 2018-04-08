using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class CollapsingPlatform : MonoBehaviour {

    [SerializeField] private float collapseWaitTime;

    private GameObject platform;
    private Renderer rend;
    private Collider coll;
    private float collapseTimer;
    private bool timerStarted;

	private void Awake ()
    {
        platform = transform.parent.gameObject;
        rend = platform.GetComponent<Renderer>();
        coll = platform.GetComponent<Collider>();
	}
	
	void Update ()
    {
        if (timerStarted)
        {
            collapseTimer += Time.deltaTime;

            if (collapseTimer > collapseWaitTime)
            {
                rend.enabled = false;
                coll.enabled = false;
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

    public void ResetPlatform()
    {
        rend.enabled = true;
        coll.enabled = true;
        timerStarted = false;
        collapseTimer = 0f;
    }
}
