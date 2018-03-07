using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class BallTriggerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BallTrigger()
    {
        Debug.Log("BallTriggerTestistä kutsuttu BallTrigger");
        transform.position = transform.position - new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
    }
}
