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

    [SerializeField] private float accelerationThreshold;
    [SerializeField] private float speedThreshold;
    private float previousSpeed;
    private float speed;
    private float speedDiff;
    private Vector3 horizontalVelocity;
    private Rigidbody rb;
    
    //Äänten feidaus
    [SerializeField] private float fadeDuration;
    private AudioSource[] aSources;
    private IEnumerator[] fader = new IEnumerator[2];
    private int activeSourceIndex = 0;
    private int volumeChangesPerSecond = 15;
    
    [SerializeField]
    private float volume;
    public float Volume
    {
        get
        {
            return volume;
        }
        set
        {
            volume = value;
        }
    }

    //todo audiocheck threshold sen hetkisestä nopeudesta riippuvaiseksi?
    //todo kiihdytys ääni pois jos nopeus liian hidas?
    //todo ehkä invokerepeating pois ja fixedupdateen checkaudio? Tai invokerepeating väli pienemmäksi
    void CheckAudio()
    {
        Debug.Log("Checked Audio");
        horizontalVelocity = rb.velocity - new Vector3(0, rb.velocity.y, 0);
        speed = horizontalVelocity.magnitude;
        speedDiff = speed - previousSpeed;

        if (speedDiff < -accelerationThreshold) Play(deceleration);
        else if (speedDiff > accelerationThreshold) Play(acceleration);
        else
        {
            if (speed < speedThreshold) Play(regularDriveSlow);
            else Play(regularDriveFast);
        }

        previousSpeed = speed;
    }

    private void Awake()
    {
        //Generate the two AudioSources
        aSources = new AudioSource[2]{
            gameObject.AddComponent<AudioSource>(),
            gameObject.AddComponent<AudioSource>()
        };

        //Set default values
        foreach (AudioSource s in aSources)
        {
            s.loop = true;
            s.playOnAwake = false;
            s.volume = 0.0f;
        }

        rb = GetComponent<Rigidbody>();
        InvokeRepeating("CheckAudio", 1f, 0.2f);
    }

    public void Play(AudioClip clip)
    {
        if (clip == aSources[activeSourceIndex].clip)
        {
            return;
        }

        //Lopettaa kaikki Coroutinet
        foreach (IEnumerator i in fader)
        {
            if (i != null)
            {
                StopCoroutine(i);
            }
        }

        //Feidaa pois aktiivisen audion
        if (aSources[activeSourceIndex].volume > 0)
        {
            fader[0] = FadeAudioSource(aSources[activeSourceIndex], fadeDuration, 0.0f, () => { fader[0] = null; });
            StartCoroutine(fader[0]);
        }

        //Feidaa sisään uuden audion
        int NextPlayer = (activeSourceIndex + 1) % aSources.Length;
        aSources[NextPlayer].clip = clip;
        aSources[NextPlayer].Play();

        fader[1] = FadeAudioSource(aSources[NextPlayer], fadeDuration, Volume, () => { fader[1] = null; });
        StartCoroutine(fader[1]);
        
        activeSourceIndex = NextPlayer;
    }
    
    //Feidaa audion sen hetkisestä äänenvoimakkuudesta targetVolumeen ajassa duration
    IEnumerator FadeAudioSource(AudioSource player, float duration, float targetVolume, System.Action finishedCallback)
    {
        int Steps = (int)(volumeChangesPerSecond * duration);
        float StepTime = duration / Steps;
        float StepSize = (targetVolume - player.volume) / Steps;

        //Lisää ääntä StepSizen verran ja odottaa StepTimen
        for (int i = 1; i < Steps; i++)
        {
            player.volume += StepSize;
            yield return new WaitForSeconds(StepTime);
        }
        player.volume = targetVolume;

        //Callback
        if (finishedCallback != null)
        {
            finishedCallback();
        }
    }
}
