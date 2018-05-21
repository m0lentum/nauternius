using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera orbitCamera;
    private bool mouseLook;

    // Use this for initialization
    void Start()
    {
        mainCamera.enabled = true;
        orbitCamera.enabled = false;
        mouseLook = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetButtonDown("CameraControl") && !mouseLook) mouseLook = true;
        else if (Input.GetButtonDown("CameraControl") && mouseLook) mouseLook = false;


        if (mouseLook)
        {
            mainCamera.enabled = false;
            orbitCamera.enabled = true;
        }
        if (!mouseLook)
        {
            orbitCamera.enabled = false;
            mainCamera.enabled = true;
            orbitCamera.transform.position = orbitCamera.transform.parent.TransformPoint(0, 1.5f, -4f);
        }
    }
}
