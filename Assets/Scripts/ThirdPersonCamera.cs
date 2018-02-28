using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    /*public GameObject target;
    public float damping = 1;
    Vector3 offset;*/
    
    public Transform target;
    public float distance;
    public float height; // poistoon ja asetus editorista?
    public float damping;
    public float rotationDamping;
    
    //test
    public float playerMinusCameraZ;


    void Start()
    {
        distance = 9f;
        height = 3f;
        damping = 10f;
        rotationDamping = 7f;
        target = GameObject.Find("Player").transform;
    }

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

