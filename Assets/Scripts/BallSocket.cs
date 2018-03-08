using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

//Täysin keskeneräinen scripti ja error handling tässä on Jeesuksen käsissä.
public class BallSocket : MonoBehaviour {
    
    [SerializeField] private GameObject targetObj;
    [SerializeField] private float maxPullForce;
    [SerializeField] private float minPullForce;
    [SerializeField] private float maxSocketTimer;
    [SerializeField] private float socketedDistance;

    public float curPullForce;
    public float socketTimer;
    private Rigidbody ballRb;

    private bool isOn;
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            if (isOn != value)
            {
                isOn = value;
                TriggerTargetObject();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ball")
        {
            curPullForce = maxPullForce;
            ballRb = other.GetComponent<Rigidbody>();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "ball")
        {
            curPullForce -= 0.2f;
            if (curPullForce < minPullForce) curPullForce = minPullForce;

            if (CheckSocketStatus(other.transform)) IsOn = true;
            
            ballRb.AddForce(ReverseVectorMagnitude(transform.position - other.transform.position) * curPullForce);

            //Debug.Log(Vector3.Distance(otherObj.transform.position, transform.position));
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

    //Tekee pienistä vektoreista isoja ja päinvastoin.
    Vector3 ReverseVectorMagnitude(Vector3 vect)
    {
        return (1 / vect.magnitude * vect);
    }

}
