using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

//todo animaatio pitää muuttaa, ehkä helpompi käyttää dropelevator-funktiota
//nyt droppi alottaa ylimmästä kohdasta (y=40) vaikka platform ei olisikan siellä. 
//Jos liikutetaan parentin positiota niin paljon alas kuin platform on dropin alkaessa, se veisi platformin maan alle (y=alkuy-40)
//miltähän näyttäisi jos enableisi rigidbodyn kun switchoffataan?
public class ElevatorSwitch : MonoBehaviour {

    [SerializeField] private Transform target;
    [SerializeField] private float stayOnTime;
    private bool timerStarted;
    private float onTimer;
    private float elevatorStartHeight;

	void Start ()
    {
	}

	void Update ()
    {
		if (timerStarted)
        {
            onTimer += Time.deltaTime;
            if (onTimer > stayOnTime)
            {
                target.GetComponent<Elevator>().SwitchState();
                timerStarted = false;
                onTimer = 0f;
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
            }
        }
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("collider tag : " + col.GetComponent<Collider>().tag);
            timerStarted = true;
            target.GetComponent<Elevator>().SwitchState();
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
        }
    }
}
