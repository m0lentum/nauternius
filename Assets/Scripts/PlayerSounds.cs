using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class PlayerSounds : MonoBehaviour {

    [SerializeField] AudioClip acceleration;
    [SerializeField] AudioClip deceleration;
    [SerializeField] AudioClip regularDriveFast;
    [SerializeField] AudioClip regularDriveSlow;

    [SerializeField] private float audioSwitchThreshold;
    [SerializeField] private float speedThreshold;

    public float previousSpeed;
    public float speed;
    public float speedDiff;
    Vector3 horizontalVelocity;
    public AudioSource aSource;
    public Rigidbody rb;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        InvokeRepeating("CheckAudio", 1f, 0.2f);
    }

    //todo audioswitchthreshold sen hetkisestä nopeudesta riippuvaiseksi??
    void CheckAudio()
    {
        Debug.Log("Checked Audio");
        horizontalVelocity = rb.velocity - new Vector3(0, rb.velocity.y, 0);
        speed = horizontalVelocity.magnitude;
        speedDiff = speed - previousSpeed;

        if (speedDiff < -audioSwitchThreshold) SwitchAudioClip(deceleration);
        else if (speedDiff > audioSwitchThreshold) SwitchAudioClip(acceleration);
        else
        {
            if (speed < speedThreshold) SwitchAudioClip(regularDriveSlow);
            else SwitchAudioClip(regularDriveFast);
        }

        previousSpeed = speed;
        
    }

    private void SwitchAudioClip(AudioClip clip)
    {
        Debug.Log("kutsuttiin switch");
        if (aSource.clip != clip)
        {
            aSource.clip = clip;
            aSource.Play();
        }
    }
}
