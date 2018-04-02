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
    private float collapseTimer;
    private bool timerStarted;

	private void Awake ()
    {
        platform = transform.parent.gameObject;
        rend = platform.GetComponent<Renderer>();
	}
	
	void Update ()
    {
        if (timerStarted)
        {
            collapseTimer += Time.deltaTime;

            if (collapseTimer > collapseWaitTime) rend.enabled = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timerStarted = true;
        }
    }
}
