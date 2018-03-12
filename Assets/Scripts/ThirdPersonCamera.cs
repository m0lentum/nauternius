using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class ThirdPersonCamera : MonoBehaviour {

    /*public GameObject target;
    public float damping = 1;
    Vector3 offset;*/
    
    [SerializeField] private Transform target;
    [SerializeField] private float distance;
    [SerializeField] private float height; // poistoon ja asetus editorista?
    [SerializeField] private float damping;
    [SerializeField] private float rotationDamping;
    
    //test
    private float playerMinusCameraZ;


    void FixedUpdate()
    {
        //Ei toimi hyvin ilman deltaTimeä, vaikka onkin fixedUpdatessa???
        //Ei myöskään toimi hyvin LateUpdatessa????

        Vector3 wantedPosition = target.TransformPoint(0, height, -distance);
        transform.position = Vector3.Lerp(transform.position, wantedPosition, damping * Time.deltaTime);

        Quaternion wantedRotation = target.rotation;
        //Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up); //vähän erilainen
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationDamping * Time.deltaTime);

        playerMinusCameraZ = target.position.z - transform.position.z; //testi
    }

    //Toisenlainen, ei oikein onnannu
    /*
    void LateUpdate()
    {
        float currentAngleX = transform.eulerAngles.x;
        float desiredAngleX = target.transform.eulerAngles.x;
        float angleX = Mathf.LerpAngle(currentAngleX, desiredAngleX, Time.deltaTime * damping);
        float currentAngleY = transform.eulerAngles.y;
        float desiredAngleY = target.transform.eulerAngles.y;
        float angleY = Mathf.LerpAngle(currentAngleY, desiredAngleY, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0, angleY, 0);
        transform.position = target.transform.position - (rotation * offset);

        transform.LookAt(target.transform);
    }*/
}

