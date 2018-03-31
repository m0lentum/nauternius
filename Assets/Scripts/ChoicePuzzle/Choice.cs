using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour {

    [SerializeField] private bool isCorrect;
    [SerializeField] private ChoiceGateController controller;

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            controller.Answer(isCorrect);
        }
    }
}
