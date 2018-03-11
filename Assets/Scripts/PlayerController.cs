﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Header("Hover")]
    [Range(0, 10)]
    [SerializeField] private float hoverHeight;
    [Range(0, 5)]
    [SerializeField] private float groundFollowHeight; // korkeus jolla alus mukautuu maan muotoihin mutta ei leiju
    [Range(0, 10)]
    [SerializeField] private float maxHoverSpeed;
    [Range(0, 5)]
    [SerializeField] private float maxHoverAcceleration;
    [Range(0, 5)]
    [SerializeField] private float angleAdjustSpeed;
    [Range(0, 3)]
    [SerializeField] private float hoverProbeDistance; // leijumiseen käytettävien spherecastien välimatka keskipisteestä
    private Vector3 hoverProbeOffset;
    [Range(0, 1)]
    [SerializeField] private float hoverProbeRadius;

    [Header("Movement")]
    [Range(0, 5)]
    [SerializeField] private float thrust; // kiihdytysteho
    [Range(0, 100)]
    [SerializeField] private float maxSpeed;
    private float trueMaxSpeed;

    [Range(2, 5)]
    [SerializeField] private float turnSpeed; // kuinka nopeasti alus kääntyy ohjauksesta
    [Range(0, 1)]
    [SerializeField] private float airControl;

    [Range(0, 60)]
    [SerializeField] private float rollAngle; // sivuttainen kallistuskulma kääntyessä
    [Range(0, 1)]
    [SerializeField] private float rollSpeed;
    
    [Header("Other")]
    [SerializeField] private float jumpForce;
    private int jumpTimer;
    [SerializeField] private int jumpWaitTime; // hypyn cooldown fixedDeltaTime-intervalleina (frameina)

    private bool hasSuperSpeed;
    private bool hasJumpAbility;

    private bool isGrounded;

    private Rigidbody rb;
    [SerializeField] private CheckPointManager checkPointManager;


    // update-loopissa käytettäviä muuttujia
    private float hInput;
    private float vInput;
    private RaycastHit hitFront;
    private RaycastHit hitBack;
    private bool didHitFront;
    private bool didHitBack;
    float targetRoll;
    float currentRoll;
    Vector3 targetVelocity;

    private const int layerMask = 1 << 8; // maski estää osumat muihin kuin terrain-layerin objekteihin


    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        hoverProbeOffset = new Vector3(0, 0, hoverProbeDistance);
        trueMaxSpeed = maxSpeed;
        jumpTimer = 0;
        hasSuperSpeed = false;
        hasJumpAbility = false;
        isGrounded = false;
	}

    void FixedUpdate()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        isGrounded = false;

        // leijuminen

        didHitFront = Physics.SphereCast(transform.TransformPoint(hoverProbeOffset), hoverProbeRadius, Vector3.down, out hitFront, hoverHeight + groundFollowHeight, layerMask);
        didHitBack = Physics.SphereCast(transform.TransformPoint(-hoverProbeOffset), hoverProbeRadius, Vector3.down, out hitBack, hoverHeight + groundFollowHeight, layerMask);

        if (didHitFront && didHitBack)
        {
            // ollaan kokonaan maassa

            transform.Rotate((hitFront.distance - hitBack.distance) * angleAdjustSpeed, 0, 0, Space.Self);

            // pystysuuntainen liikenopeus
            float minDistance = Mathf.Min(hitFront.distance, hitBack.distance);
            if (minDistance <= hoverHeight)
            {
                float targetVel = (hoverHeight - minDistance) * maxHoverSpeed;
                float velDiff = targetVel - rb.velocity.y;
                if (velDiff < maxHoverAcceleration)
                {
                    rb.velocity = new Vector3(rb.velocity.x, targetVel, rb.velocity.z);
                }
                else
                {
                    rb.velocity = rb.velocity + new Vector3(0, maxHoverAcceleration, 0);
                }

                isGrounded = true;
            }
        }
        else if (didHitFront)
        {
            // jos vain toinen pää on tason päällä, pitäisi kääntää jonkin verran ettei tule tiettyjä jumitilanteita (todo)
        }
        else if (didHitBack)
        {
            
        }
        else
        {
            // ollaan (ainakin osittain) ilmassa, käännetään alusta jonkin verran liikesuunnan mukaan

            
        }

        // kallistus

        targetRoll = -hInput * rollAngle;
        float rollToApply = (targetRoll - currentRoll) * rollSpeed;
        transform.Rotate(0, 0, rollToApply, Space.Self);
        currentRoll += rollToApply;

        // kiihdyttäminen ja kääntyminen

        if (isGrounded)
        {
            rb.velocity = Vector3.ProjectOnPlane(rb.velocity, new Vector3(transform.right.x, 0, transform.right.z)) + transform.forward * vInput * thrust;
            transform.Rotate(0, hInput * turnSpeed, 0, Space.World);
        }
        else
        {
            rb.velocity = Vector3.ProjectOnPlane(rb.velocity, new Vector3(transform.right.x, 0, transform.right.z)) + Vector3.ProjectOnPlane(transform.forward * vInput * thrust * airControl, Vector3.up);
            transform.Rotate(0, hInput * turnSpeed * airControl, 0, Space.World);
        }


        if (rb.velocity.z * rb.velocity.z + rb.velocity.x * rb.velocity.x > trueMaxSpeed * trueMaxSpeed)
        {
            rb.velocity *= trueMaxSpeed / rb.velocity.magnitude;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject colObject = collision.collider.gameObject;
        Debug.Log("Osui :" + colObject.name);
        
        if (colObject.tag == "wall") SuperSpeedOff();
    }

    public void SuperSpeedOn()
    {
        if (!hasSuperSpeed)
        {
            hasSuperSpeed = true;
            trueMaxSpeed = 3 * maxSpeed;
        }
    }

    public void SuperSpeedOff()
    {
        if (hasSuperSpeed)
        {
            hasSuperSpeed = false;
            trueMaxSpeed = maxSpeed;
        }
    }

    public void Jump()
    {
        rb.AddRelativeForce(0f, jumpForce, 0f, ForceMode.Impulse);
        jumpTimer = 0;
    }

    public void Die()
    {
        Debug.Log("Kuoli saatana");
        transform.position = checkPointManager.CurCheckPoint.spawnPoint;
    }

    public void EnableJumpAbility()
    {
        Debug.Log("Nyt voi hypätä");
        hasJumpAbility = true;
    }

    // Spherecastien debug-piirto
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        if (didHitFront)
        {
            Vector3 origin = transform.TransformPoint(hoverProbeOffset);
            Gizmos.DrawRay(origin, Vector3.down * hitFront.distance);
            if (isGrounded) Gizmos.DrawWireSphere(origin + Vector3.down * hitFront.distance, hoverProbeRadius);
        }

        if (didHitBack)
        {
            Vector3 origin = transform.TransformPoint(-hoverProbeOffset);
            Gizmos.DrawRay(origin, Vector3.down * hitBack.distance);
            if (isGrounded) Gizmos.DrawWireSphere(origin + Vector3.down * hitBack.distance, hoverProbeRadius);
        }

        //Gizmos.DrawRay(transform.TransformPoint(Vector3.zero), transform.forward * 20.0f);
    }

    // päivitetään vastaavat vektorit kun editorissa muutetaan arvoja
    void OnValidate()
    {
        hoverProbeOffset = new Vector3(0, 0, hoverProbeDistance);
    }
}
