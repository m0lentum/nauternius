using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

//Keskeneräinen, kokeileva, eloisa, hieman hapettunut skripti. Vahva hapan jälkimaku.
//Force Field ei toimikaan uuden liikkumisen kanssa.
public class BallSocket : MonoBehaviour {

    public delegate void SocketActivation();
    public event SocketActivation OnSocketActivated;

    [SerializeField] private GameObject targetObj;
    [SerializeField] private GameObject targetBall;

    [SerializeField] private float maxPullForce;
    [SerializeField] private float minPullForce;
    [SerializeField] private float maxSocketTimer;
    [SerializeField] private float socketedDistance;
    [SerializeField] private float forceFieldPower;

    [SerializeField] private Material activatedMaterial;

    [SerializeField] private AudioClip correctBallSound;
    [SerializeField] private AudioClip incorrectBallSound;
    [SerializeField] private AudioClip resetSound;

    //publicit pois testin jälkeen
    private float curPullForce;
    public float socketTimer;
    private bool isForceFieldActive;
    public bool isActive;
    private GameObject ball;
    private Rigidbody ballRb;
    private Rigidbody collRb;
    private AudioSource aSource;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
    }

    //Ottaa lähelle tulevan objektin rigidbodyn talteen
    private void OnTriggerEnter(Collider other)
    {
        //Ottaa talteen ensimmäisen pallon ja sen RigidBodyn
        if (other.CompareTag("Ball") && !isForceFieldActive)
        {
            curPullForce = maxPullForce;
            ball = other.gameObject;
            ballRb = other.GetComponent<Rigidbody>();
        }
        //Ottaa talteen törmäävän objektin RigidBodyn, kun ForceField on päällä
        else
        {
            collRb = GetParentWithRb(other.transform);
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Imee palloja keskelle, jos ForceField ei ole päällä
        if (other.gameObject == ball)
        {
            curPullForce -= 0.2f;
            if (curPullForce < minPullForce) curPullForce = minPullForce;
            if (CheckSocketStatus(other.gameObject)) ActivateSocket(other.gameObject);
            ballRb.AddForce((transform.position - other.transform.position) * curPullForce);
        }
        //Työntää viimeiseksi osunutta objektia pois jos ForceField päällä
        else if (isForceFieldActive)
        {
            Vector3 awayDirection = (other.transform.position - transform.position).normalized;
            awayDirection = awayDirection - new Vector3(0, awayDirection.y, 0);
            if(collRb != null) collRb.AddForce(awayDirection * (collRb.velocity.magnitude + 10) * forceFieldPower);
        }
    }
    
    void TriggerTargetObject()
    {
        targetObj.GetComponent<BallTriggerObject>().BallTrigger();
    }

    //Katsoo onko oikea objekti paikallaan socketissa ja jos on ollut tarpeeksi kauan paikallaan, palauttaa True.
    bool CheckSocketStatus(GameObject socketedObj)
    {
        if (socketTimer > maxSocketTimer) return true;

        if (Vector3.Distance(socketedObj.transform.position, transform.position) < socketedDistance) socketTimer += Time.deltaTime;
        else socketTimer = 0;

        return false;
    }

    //Aktivoi ForceFieldin ja jos oikea pallo, invokee eventin
    void ActivateSocket(GameObject socketedObj)
    {
        if (isForceFieldActive) return;

        isForceFieldActive = true;
        //aSource.PlayOneShot(ÄäniKunPalloKiinnittyySockettiin)

        if (socketedObj == targetBall)
        {
            isActive = true;
            OnSocketActivated.Invoke();
        }
    }

    //Nollaa soketin muuttujat ja pelaaja pystyy työntämään pallon pois siitä - todo varmaan parempikin tapa tähän
    public void ResetSocket()
    {
        ball = null; ballRb = null; collRb = null;
        isForceFieldActive = false;
        isActive = false;
        socketTimer = 0;
        aSource.PlayOneShot(resetSound);
    }

    //Tekee pienistä vektoreista isoja ja päinvastoin.
    Vector3 ReverseVectorMagnitude(Vector3 vect)
    {
        return (1 / vect.magnitude * vect);
    }

    //Palauttaa ensimmäisen objektin rigidbodyn, tai sen ensimmäisen vanhemman rigidbodyn, jolla sellainen on
    private Rigidbody GetParentWithRb(Transform childTr)
    {
        Transform tr = childTr;
        while (tr != null)
        {
            Rigidbody rb = tr.GetComponent<Rigidbody>();
            if (rb != null)
            {
                return rb;
            }
            tr = tr.parent;
        }

        return null; //Ei vanhempaa, jolla rigidbody
    }
}
