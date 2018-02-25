using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    public delegate void MyDelegate(CheckPoint cp);
    public event MyDelegate onTrigger;

    public bool triggered;
    public Vector3 spawnPoint;
    //public Vector3 

	// Use this for initialization
	void Start ()
    {
        spawnPoint = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.tag == "Player")
            {
                //cPos = transform.position;
                Debug.Log("Triggered");
                Trigger();
            }
        }
    }

    void Trigger()
    {
        triggered = true;
        onTrigger.Invoke(this);
    }
}
