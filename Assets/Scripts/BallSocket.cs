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
    [SerializeField] private float maxPullForce;
    [SerializeField] private float minPullForce;
    [SerializeField] private float maxSocketTimer;
    [SerializeField] private float socketedDistance;
    [SerializeField] private float forceFieldPower;
    [SerializeField] private Material activatedMaterial;

    private float curPullForce;
    private float socketTimer;
    private bool isForceFieldActive;
    private Rigidbody ballRb;
    private Rigidbody collRb;

    //Ottaa lähelle tulevan objektin rigidbodyn talteen
    //Bugaa helposti, jos forcefield päällä ja 2 objektia tulee sisään samaan aikaan, koska ottaa uuden RBn talteen ennen kuin vanha on käsitelty. Onko paha?
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ball" && !isForceFieldActive)
        {
            curPullForce = maxPullForce;
            ballRb = other.GetComponent<Rigidbody>();
        }
        else collRb = other.transform.root.GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider other)
    {
        //Työntää pelaajan pois jos force field on päällä eli socket on aktiivinen
        if (isForceFieldActive && other.tag != "ball")
        {
            Vector3 awayDirection = (other.transform.position - transform.position).normalized;
            awayDirection = awayDirection - new Vector3(0, awayDirection.y, 0);
            if(collRb != null) collRb.AddForce(awayDirection * (collRb.velocity.magnitude + 10) * forceFieldPower);
        }

        //Imee palloa sisään päin
        if (other.tag == "ball")
        {
            curPullForce -= 0.2f;
            if (curPullForce < minPullForce) curPullForce = minPullForce;
            if (CheckSocketStatus(other.transform)) ActivateSocket();
            ballRb.AddForce((transform.position - other.transform.position) * curPullForce);
        }
    }
    
    void TriggerTargetObject()
    {
        targetObj.GetComponent<BallTriggerObject>().BallTrigger();
    }

    //Katsoo onko objekti paikallaan socketissa ja jos on ollut tarpeeksi kauan paikallaan, palauttaa True.
    bool CheckSocketStatus(Transform obj)
    {
        if (socketTimer > maxSocketTimer) return true;

        if (Vector3.Distance(obj.position, transform.position) < socketedDistance) socketTimer += Time.deltaTime;
        else socketTimer = 0;

        return false;
    }

    void ActivateSocket()
    {
        if (isForceFieldActive) return;
        ballRb.gameObject.GetComponent<Renderer>().material = activatedMaterial;
        isForceFieldActive = true;
        TriggerTargetObject();
    }

    void DeactivateSocket()
    {
        //Tarvitaan tai sitten ei?
    }

    //Tekee pienistä vektoreista isoja ja päinvastoin.
    Vector3 ReverseVectorMagnitude(Vector3 vect)
    {
        return (1 / vect.magnitude * vect);
    }

}
