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

    [SerializeField] private Transform targetElevator;
    [SerializeField] private float stayOnTime;
    private Animator anim;
    private bool timerStarted;
    private float onTimer;
    private float elevatorStartHeight;

	void Start ()
    {
        anim = targetElevator.GetComponent<Animator>();
        elevatorStartHeight = targetElevator.position.y;
	}

	void Update ()
    {
		if (timerStarted)
        {
            onTimer += Time.deltaTime;
            if (onTimer > stayOnTime) SwitchOff();
        }
	}

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            anim.SetBool("IsOn", true);
            timerStarted = true;
        }
    }
    
    private void SwitchOff()
    {
        anim.SetBool("IsOn", false);
        timerStarted = false;
        onTimer = 0;
        //StartCoroutine(DropElevator());
    }

    IEnumerator DropElevator()
    {
        while (targetElevator.position.y > elevatorStartHeight + 0.04f)
        {
            targetElevator.position -= new Vector3(0, 0.2f, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
