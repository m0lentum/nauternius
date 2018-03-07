using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class CheckPoint : MonoBehaviour {

    public delegate void MyDelegate(CheckPoint cp);
    public event MyDelegate onTrigger;

    private bool triggered;

    private Vector3 spawnPoint;
    public Vector3 SpawnPoint { get { return spawnPoint; } }
    
    void Start ()
    {
        spawnPoint = transform.position;
    }
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.tag == "Player") Trigger();
    }

    void Trigger()
    {
        triggered = true;
        onTrigger.Invoke(this);
    }
}
