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

    public Vector3 cameraRelative;

    private void Awake()
    {
        initDamping = 7;
        damping = initDamping;
    }
    void FixedUpdate()
    {

        cameraRelative = target.InverseTransformPoint(transform.position);
        float relZ = cameraRelative.z + 9;
        if (cameraRelative.z > -8) damping = (1 + relZ) * initDamping;
        else damping = initDamping;
        
        Vector3 wantedPosition = target.TransformPoint(0, height, -distance);
        transform.position = Vector3.Lerp(transform.position, wantedPosition, damping * Time.deltaTime);
        
        Quaternion wantedRotation = target.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationDamping * Time.deltaTime);

        /*float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;

        if (horizontal != 0)
        {
            transform.Rotate(0, horizontal, 0);
        }
        else
        {
        Quaternion wantedRotation = target.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationDamping * Time.deltaTime);
        }*/
    }
}

