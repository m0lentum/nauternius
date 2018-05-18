using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngineController : MonoBehaviour {

    [SerializeField] private bool isBack;
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private float maxAngle;
    private float currentAngle;
    private float targetAngle;

    private UVScroller flameScroller;

	void Start ()
    {
        flameScroller = GetComponentInChildren<UVScroller>();
	}

	void Update ()
    {
        if (isBack)
        {
            currentAngle = transform.localEulerAngles.x;
            if (currentAngle > 180.0f) currentAngle -= 360.0f;
            targetAngle = (Input.GetAxis("Forward") - Input.GetAxis("Reverse")) * maxAngle;

            float diff = targetAngle - currentAngle;
            if (Mathf.Abs(diff) < maxTurnSpeed * Time.deltaTime) transform.Rotate(new Vector3(diff, 0, 0));
            else transform.Rotate(new Vector3(Mathf.Sign(diff) * maxTurnSpeed * Time.deltaTime, 0, 0));
        }
        else
        {
            // rumaa toistoa mutta en nyt keksi parempaakaan
            currentAngle = transform.localEulerAngles.z;
            if (currentAngle > 180.0f) currentAngle -= 360.0f;
            targetAngle = -Input.GetAxis("Steer") * maxAngle;

            float diff = targetAngle - currentAngle;
            if (Mathf.Abs(diff) < maxTurnSpeed * Time.deltaTime) transform.Rotate(new Vector3(0, 0, diff));
            else transform.Rotate(new Vector3(0, 0, Mathf.Sign(diff) * maxTurnSpeed * Time.deltaTime));
        }
	}
}
