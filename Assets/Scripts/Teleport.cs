using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class Teleport : MonoBehaviour {

	[SerializeField] private Vector3 teleportEndPos;
	[SerializeField] private GameObject player;
    [SerializeField] private Canvas flashCanvas;

    private FlashCanvas flashScript;
    private AudioSource aSource;

    private void Start()
    {
        flashScript = flashCanvas.GetComponent<FlashCanvas>();
        aSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            aSource.Play();
            StartCoroutine(UseTeleport());
        }
    }

    private IEnumerator UseTeleport()
    {
        flashScript.Flash(Color.cyan, 0.2f);
        yield return new WaitForSeconds(0.2f);
        player.transform.position = teleportEndPos;
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
}
