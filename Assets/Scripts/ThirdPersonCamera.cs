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
    public float damping;
    public float initDamping;
    [SerializeField] private float rotationDamping;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float bumperDistanceCheck = 2.5f; // length of bumper ray
    [SerializeField] private float bumperCameraHeight = 1.0f; // adjust camera height while bumping
    [SerializeField] private Vector3 bumperRayOffset; // allows offset of the bumper ray from target origin

    public Vector3 cameraRelative;
    
    void FixedUpdate()
    {
        cameraRelative = target.InverseTransformPoint(transform.position);
        float relZ = cameraRelative.z + distance;
        if (relZ > 1) damping = relZ * initDamping;
        else damping = initDamping;
        
        Vector3 wantedPosition = target.TransformPoint(0, height, -distance);

        RaycastHit hit;
        Vector3 back = target.transform.TransformDirection(-1 * Vector3.forward);
        if (Physics.Raycast(target.TransformPoint(bumperRayOffset), back, out hit, bumperDistanceCheck))
        {
            wantedPosition.x = hit.point.x;
            wantedPosition.y = wantedPosition.y = Mathf.Lerp(hit.point.y + bumperCameraHeight, wantedPosition.y, Time.deltaTime * damping);
            wantedPosition.z = hit.point.z;
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, damping * Time.deltaTime);

        Quaternion wantedRotation = target.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationDamping * Time.deltaTime);
    }
}

