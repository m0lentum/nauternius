using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class InvisibilityFilter : MonoBehaviour {

    //Tähän asetetaan tyhjä objekti, jonka lapsina ovat kaikki filtterin vaikutuksessa olevat objektit
    [SerializeField] private Transform affectedObjectsParent;
    //[SerializeField] private Text helpText;
    private PlayerController pController;
    
    void Awake ()
    {
        pController = GameObject.Find("Player").GetComponent<PlayerController>();
        pController.OnFilterChanged += TriggerObjects;
	}
	
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pController.HasFilter = true;
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<ParticleSystem>().Stop();
            //helpText.gameObject.SetActive(true);
            //StartCoroutine(DisableAfterWait(helpText.gameObject));
        }
    }

    private void TriggerObjects(bool filterOn)
    {
        foreach(Transform tr in affectedObjectsParent)
        {
            foreach (Renderer rend in tr.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = filterOn;
            }
        }
    }

    IEnumerator DisableAfterWait(GameObject obj)
    {
        yield return new WaitForSeconds(4);
        obj.SetActive(false);
    }
}
