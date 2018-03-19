using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

//Keskeneräinen, kokeileva, eloisa, hieman hapettunut skripti. Vahva hapan jälkimaku.
//Force Field ei toimikaan uuden liikkumisen kanssa.
public class BallSocket : MonoBehaviour {
    
    [SerializeField] private GameObject targetObj;
    [SerializeField] private GameObject targetBall;

    [SerializeField] private float maxPullForce;
    [SerializeField] private float minPullForce;
    [SerializeField] private float maxSocketTimer;
    [SerializeField] private float socketedDistance;
    [SerializeField] private float forceFieldPower;
    [SerializeField] private Material activatedMaterial;

    //publicit pois testin jälkeen
    private float curPullForce;
    public float socketTimer;
    public bool isForceFieldActive;
    public GameObject ball;
    public Rigidbody ballRb;
    public Rigidbody collRb;

    //Ottaa lähelle tulevan objektin rigidbodyn talteen
    //Bugaa helposti, jos forcefield päällä ja 2 objektia tulee sisään samaan aikaan(?), koska ottaa uuden RBn talteen ennen kuin vanha on käsitelty. Onko paha?
    private void OnTriggerEnter(Collider other)
    {
        //Ottaa talteen pallon ja sen RigidBodyn
        if (other.tag == "ball" && !isForceFieldActive)
        {
            curPullForce = maxPullForce;
            ball = other.gameObject;
            ballRb = other.GetComponent<Rigidbody>();
        }
        //Ottaa talteen törmäävän objektin RigidBodyn, kun ForceField on päällä
        //todo ei toimi, jos rigidbody ei ole hierarkian ylimmässä objektissa
        else
        {
            Debug.Log(other.transform.root.name);
            collRb = other.transform.root.GetComponent<Rigidbody>();
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
        //if (socketedObj != targetBall) return false;
        if (socketTimer > maxSocketTimer) return true;

        if (Vector3.Distance(socketedObj.transform.position, transform.position) < socketedDistance) socketTimer += Time.deltaTime;
        else socketTimer = 0;

        return false;
    }

    //Aktivoi ForceFieldin ja jos oikea pallo, näyttää sen efektillä ja triggeröi targetobjektin
    void ActivateSocket(GameObject socketedObj)
    {
        if (isForceFieldActive) return;
        isForceFieldActive = true;

        if (socketedObj == targetBall)
        {
            ballRb.gameObject.GetComponent<Renderer>().material = activatedMaterial;
            TriggerTargetObject();
        }


        /*if (socketedObj == targetBall)
        {
            ballRb.gameObject.GetComponent<Renderer>().material = activatedMaterial;
            isForceFieldActive = true;
            TriggerTargetObject();
        }*/
    }

    //Nollaa soketin muuttujat ja pelaaja pystyy työntämään pallon pois siitä
    void ResetSocket()
    {
        ball = null; ballRb = null; collRb = null;
        isForceFieldActive = false;
        socketTimer = 0;
    }

    //Testi, Reset erilliseen pelimaailman objektiin(?)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) ResetSocket();
    }

    //Tekee pienistä vektoreista isoja ja päinvastoin.
    Vector3 ReverseVectorMagnitude(Vector3 vect)
    {
        return (1 / vect.magnitude * vect);
    }

}
