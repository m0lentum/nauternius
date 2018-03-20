using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class BallSocketReset : MonoBehaviour {

    [SerializeField] private Transform ballSocketParent;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ResetBallSockets();
        }
    }

    void ResetBallSockets()
    {
        Debug.Log("reset socketS kutsuttu");
        foreach (Transform tr in ballSocketParent)
        {
            Debug.Log(tr.name);
            tr.GetChild(0).GetComponent<BallSocket>().ResetSocket();
        }
    }
}
