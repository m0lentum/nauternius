using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class OrbitCamera : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float rotationDamping;
    [SerializeField]
    private float offsetHeight;
    [SerializeField]
    private float offsetDistance;

    void Start()
    {
        transform.position = transform.parent.TransformPoint(0, offsetHeight, -offsetDistance);
    }

    // Update is called once per frame

    void LateUpdate()
    {
        transform.RotateAround(transform.parent.position, Vector3.up, Input.GetAxis("RightStickVertical") * rotationSpeed * Time.deltaTime);
        transform.RotateAround(transform.parent.position, Vector3.right, Input.GetAxis("RightStickHorizontal") * rotationSpeed * Time.deltaTime);
        transform.RotateAround(transform.parent.position, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime);
        transform.RotateAround(transform.parent.position, Vector3.right, Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime);
        transform.LookAt(transform.parent);
    }
}