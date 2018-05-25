using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    private bool switchedOn;
    private Rigidbody colRB;

	void Start ()
    {
        switchedOn = false;
	}

    void OnTriggerStay(Collider col) {

        colRB = col.gameObject.GetComponentInParent<Rigidbody>();
        if (switchedOn)
        {
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("collider tag : " + col.GetComponent<Collider>().tag); 
                colRB.AddRelativeForce(0f, 500f, 0f, ForceMode.Impulse);
            }
        }
    }

    public void SwitchState()
    {
        switchedOn = !switchedOn;
    }
}
