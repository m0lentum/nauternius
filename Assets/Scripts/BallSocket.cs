using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

//Täysin keskeneräinen scripti ja error handling tässä on Jeesuksen käsissä.
public class BallSocket : MonoBehaviour {

    [SerializeField] private float pullForce;
    [SerializeField] private float stillSpeed;
    [SerializeField] private GameObject targetObj;

    private bool isOn;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            if (isOn != value)
            {
                isOn = value;
                TriggerTargetObject();
            }
        }
    }

    void Start ()
    {
        //targetObj = editorissa asetettu
        //stillSpeed = 0.2f;
        //pullForce = 15f;
	}

	void Update ()
    {

	}

    void OnTriggerStay(Collider other)
    {
        GameObject otherObj = other.gameObject;
        if (otherObj.tag == "ball")
        {
            otherObj.transform.position = Vector3.MoveTowards(otherObj.transform.position, transform.position, pullForce * Time.deltaTime);
        }

        //GetComponent pois jatkuvasti toistettavasta funktiosta, vois esim. hallinnoida ontriggerenterissä/exitissä?
        if (otherObj.GetComponent<Rigidbody>().velocity.magnitude < stillSpeed) IsOn = true;
        
    }

    void TriggerTargetObject()
    {
        Debug.Log("BallSocketista kutsuttu TriggerTargetObject");
        targetObj.GetComponent<BallTriggerTest>().BallTrigger();
    }

}
