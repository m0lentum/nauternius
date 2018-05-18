using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour {

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip clip;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
}
