using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

	[SerializeField] private Vector3 teleportEndPos;
	[SerializeField] private GameObject player;
    [SerializeField] private Canvas flashCanvas;

    private CanvasGroup flashCG;
    private AudioSource aSource;

    private void Start()
    {
        flashCG = flashCanvas.GetComponent<CanvasGroup>();
        aSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FlashCanvas());
            aSource.Play();
            player.transform.position = teleportEndPos;
        }
    }

    public IEnumerator FlashCanvas()
    {
        for (int i = 0; i < 10; i++)
        {
            flashCG.alpha += 0.05f;
            if (i == 10) yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 20; i++)
        {
            flashCG.alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
