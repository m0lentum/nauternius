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

    private Quaternion fromRotation;
    private Quaternion toRotation;

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
            fromRotation = otherObj.transform.rotation;
            toRotation = Quaternion.Euler(0, 0, 0);
            
            otherObj.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, 1);
            otherObj.GetComponent<Rigidbody>().AddForce((transform.position - otherObj.transform.position) * pullForce);
            //otherObj.transform.position = Vector3.MoveTowards(otherObj.transform.position, transform.position, pullForce * Time.deltaTime);

            Debug.Log(otherObj.GetComponent<Rigidbody>().velocity.magnitude);
            if (otherObj.GetComponent<Rigidbody>().velocity.magnitude < stillSpeed) IsOn = true;
        }

        //GetComponent pois jatkuvasti toistettavasta funktiosta, vois esim. hallinnoida ontriggerenterissä/exitissä?
        
    }

    void TriggerTargetObject()
    {
        Debug.Log("BallSocketista kutsuttu TriggerTargetObject");
        targetObj.GetComponent<BallTriggerTest>().BallTrigger();
    }

}
