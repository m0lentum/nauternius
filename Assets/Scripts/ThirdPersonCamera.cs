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
    private Vector3 offsetX;
    private Vector3 offsetY;
    private bool freeCamera;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float bumperDistanceCheck; 
    [SerializeField] private float bumperCameraHeight; 
    [SerializeField] private Vector3 bumperRayOffset; //BumperRayn lähtöpisteen ero pelaajan keskipisteeseen

    void Start()
    {

        offsetX = new Vector3(0, height, -distance);
        offsetY = new Vector3(0, 0, -distance);
    }

    void FixedUpdate()
    {
        if ((Input.GetAxisRaw("RightStickHorizontal") != 0) || (Input.GetAxisRaw("RightStickVertical") != 0)) freeCamera = true; 

        cameraRelative = target.InverseTransformPoint(transform.position);
        float relZ = cameraRelative.z + distance;
        if (relZ > 1) damping = relZ * initDamping;
        else damping = initDamping;
        wantedPosition = target.TransformPoint(0, height, -distance);




       if (!freeCamera)
       {
           RaycastHit hit;
           Vector3 back = target.transform.TransformDirection(-1 * Vector3.forward);

           if (Physics.Raycast(target.TransformPoint(bumperRayOffset), back, out hit, bumperDistanceCheck))
           {

               wantedPosition.x = hit.point.x;
               wantedPosition.y = Mathf.Lerp(hit.point.y + bumperCameraHeight, wantedPosition.y, Time.deltaTime * damping);
               wantedPosition.z = hit.point.z;
           }

           transform.position = Vector3.Lerp(transform.position, wantedPosition, damping * Time.deltaTime);
        }

        Quaternion wantedRotation = target.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationDamping * Time.deltaTime);
        freeCamera = false;
    }

    void LateUpdate()
    {

        transform.Translate(Input.GetAxis("RightStickHorizontal") * Vector3.up * rotationSpeed * Time.deltaTime, target);
        transform.Translate(Input.GetAxis("RightStickVertical") * -Vector3.right * rotationSpeed * Time.deltaTime, target);
        transform.LookAt(target.transform);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}

