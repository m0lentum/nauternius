using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera2 : MonoBehaviour {

    public float CameraMoveSpeed = 120f;
    public GameObject followObj;
    public Vector3 followPos;
    public float clampAngle = 80f;
    public float inputSens = 1.5f;
    public GameObject CameraObj;
    public GameObject playerObj;
    public float camDistanceToPlayerX;
    public float camDistanceToPlayerY;
    public float camDistanceToPlayerZ;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;

    public float smoothX;
    public float smoothY;
    public float rotY = 0f;
    public float rotX = 0f;


    
    void Start ()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
	}
	
	void Update ()
    {
        //float inputX = Input.GetAxis("RightStickHorizontal");
        //float inputZ = Input.GetAxis("RightStickVertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        //finalInputX = inputX + mouseX;
        //finalInputZ = inputZ + mouseY;

        rotY += mouseX * inputSens;
        rotX += mouseY * inputSens;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = localRot;
    }

    void LateUpdate()
    {
        CameraUpdater();
    }

    void CameraUpdater()
    {
        Transform target = followObj.transform;

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
