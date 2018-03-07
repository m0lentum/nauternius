using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class JumpPowerUp : MonoBehaviour {

    //Tätä ei oiekastaan tarvita, koska OnTriggerEnterissä tarkistetaan että collider on pelaaja
    private PlayerController playerController;

    void Start ()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController.hasJumpAbility = true;
            Destroy(gameObject);
        }
    }
}
