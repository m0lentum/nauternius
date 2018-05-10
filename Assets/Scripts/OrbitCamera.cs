using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour {

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float offsetHeight;
    [SerializeField] private float offsetDistance;
    [SerializeField] private float initDamping;

    private bool freeCamera;
    private Vector3 wantedPosition;
    private Vector3 cameraRelative;
    private float damping;
    private Rigidbody target;
    private float targetVel;

	// Update is called once per frame

	void LateUpdate () {
        if ((Input.GetAxisRaw("RightStickHorizontal") != 0) || (Input.GetAxisRaw("RightStickVertical") != 0)) freeCamera = true;

        target = transform.GetComponentInParent<Rigidbody>();
        
        cameraRelative = transform.parent.InverseTransformPoint(transform.position);
        float relZ = cameraRelative.z + offsetDistance;
        if (relZ > 1) damping = relZ * initDamping;
        else damping = initDamping;

        if (target.velocity.magnitude * 0.5f > 1f) targetVel = target.velocity.magnitude * 0.05f;
        else targetVel = 1;

        if (!freeCamera)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.parent.rotation, 7f * Time.deltaTime);
            wantedPosition = transform.parent.TransformPoint(0, (offsetHeight * (targetVel * 0.5f)), -(offsetDistance * targetVel));
        }
        else
        {
            transform.RotateAround(transform.parent.position, Vector3.up, Input.GetAxis("RightStickVertical") * rotationSpeed * Time.deltaTime);
            transform.RotateAround(transform.parent.position, Vector3.right, Input.GetAxis("RightStickHorizontal") * rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.parent);
            wantedPosition = transform.position;
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, damping * Time.deltaTime);

        freeCamera = false;
	}
}
