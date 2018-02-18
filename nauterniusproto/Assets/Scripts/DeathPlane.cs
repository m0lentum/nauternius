using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour {
    
    
    void Start ()
    {

	}
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Ei _pitäis_ olla mahdollisuutta erroriin, kunhan vain pelaajalla on Player-tag
            other.gameObject.GetComponent<PlayerController>().Die();
        }
    }
}
