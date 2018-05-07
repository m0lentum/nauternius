using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

    //todo kaikki
public class ThirdPersonCamera : MonoBehaviour {
    
    [SerializeField] private Transform target;
    [SerializeField] private float distance;
    [SerializeField] private float height; // poistoon ja asetus editorista?
    [SerializeField] private float initDamping;
    [SerializeField] private float rotationDamping;
    private float damping;
    private Vector3 cameraRelative;
    private Vector3 wantedPosition;
    private Vector3 fixedPosition;
    private bool freeCamera;
    private Rigidbody trb;
    private float fullRotation;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float bumperDistanceCheck; 
    [SerializeField] private float bumperCameraHeight; 
    [SerializeField] private Vector3 bumperRayOffset; //BumperRayn lähtöpisteen ero pelaajan keskipisteeseen


    void FixedUpdate()
    {
        trb = target.GetComponent<Rigidbody>();
            
        if ((Input.GetAxisRaw("RightStickHorizontal") != 0) || (Input.GetAxisRaw("RightStickVertical") != 0)) freeCamera = true; 

        cameraRelative = target.InverseTransformPoint(transform.position);
        float relZ = cameraRelative.z + distance;
        if (relZ > 1) damping = relZ * initDamping;
        else damping = initDamping;





        if (!freeCamera)
        {
            wantedPosition = target.TransformPoint(0, height, -distance);

            RaycastHit hit;
            Vector3 back = target.transform.TransformDirection(-1 * Vector3.forward);

            if (Physics.Raycast(target.TransformPoint(bumperRayOffset), back, out hit, bumperDistanceCheck))
            {

                wantedPosition.x = hit.point.x;
                wantedPosition.y = Mathf.Lerp(hit.point.y + bumperCameraHeight, wantedPosition.y, Time.deltaTime * damping);
                wantedPosition.z = hit.point.z;
            }


            Quaternion wantedRotation = target.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationDamping * Time.deltaTime);

        }
        else
        {
            if (trb.velocity.magnitude > 1) fullRotation = rotationSpeed * trb.velocity.magnitude * 0.25f;
            else fullRotation = rotationSpeed;
            transform.RotateAround(target.position, -Vector3.up, Input.GetAxis("RightStickVertical") * fullRotation * Time.deltaTime);
            transform.RotateAround(target.position, Vector3.right, Input.GetAxis("RightStickHorizontal") * fullRotation * Time.deltaTime);
            transform.LookAt(target);
            fixedPosition = (target.position - transform.position).normalized;
            wantedPosition = target.position - fixedPosition * distance;
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, damping * Time.deltaTime);



        freeCamera = false;
    }

}

