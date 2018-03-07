using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class InvisibilityFilter : MonoBehaviour {

    //Tähän asetetaan tyhjä objekti, jonka lapsina ovat kaikki filtterin vaikutuksessa olevat objektit
    [SerializeField] private Transform affectedObjectsParent;

    void Start ()
    {
	}
	
	void Update ()
    {
		
	}

    //Pitää muuttaa pelaajan colliderit tai tarkastaa, ettei joku niistä ole jo alueen sisällä
    //Muuten objektien näkyvyys vaihtuu aina kun eri osa pelaajasta collidaa triggerboxiin
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") TriggerObjects();
    }

    private void TriggerObjects()
    {
        foreach(Transform tr in affectedObjectsParent)
        {
            Renderer rend = tr.GetComponent<Renderer>();
            rend.enabled = !rend.enabled;
        }
    }
}
