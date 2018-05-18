using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class DeathPlane : MonoBehaviour {
    
    
    void Start ()
    {

	}
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        GameObject otherParent = other.transform.root.gameObject;
        if (otherParent.tag == "Player")
        {
            //Ei _pitäis_ olla mahdollisuutta erroriin, kunhan vain pelaajalla on Player-tag
            StartCoroutine(otherParent.GetComponent<PlayerController>().Die());
        }
    }
}
