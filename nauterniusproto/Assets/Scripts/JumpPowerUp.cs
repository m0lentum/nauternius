using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerUp : MonoBehaviour {

    public PlayerController playerController;

    void Start ()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("törmäs triggerboxiin");
        if (other.gameObject.tag == "Player")
        {
            playerController.hasJumpAbility = true;
            Destroy(gameObject);
        }
    }
}
