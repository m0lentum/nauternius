using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Range(0, 10)]
    [SerializeField] private float hoverHeight;
    [Range(0, 10)]
    [SerializeField] private float hoverSpeed;
    [Range(0, 3)]
    [SerializeField] private float maxHoverAcceleration;
    [Range(0, 0.5f)]
    [SerializeField] private float angleAdjustStrength;
    [Range(0, 3)]
    [SerializeField] private float hoverProbeDistance; // leijumiseen käytettävien boxcastien välimatka keskipisteestä
    private Vector3 hoverProbeOffset;
    [Range(0, 3)]
    [SerializeField] private float hoverProbeRadius;

    [Range(0, 5)]
    [SerializeField] private float thrust; // kiihdytysteho
    [SerializeField] private float maxSpeed;
    private float trueMaxSpeed;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float handling; //Vaikuttaa kuinka paljon input otetaan huomioon [0,1] //Edessä oleva kommentti ei näytä suomen kielen lauseelta

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
    private Vector3 fwd;
    private RaycastHit hitFront;
    private RaycastHit hitBack;
    private bool didHitFront;
    private bool didHitBack;

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
        fwd = transform.rotation * Vector3.forward;

        // leijuminen

        Vector3 originFront = transform.TransformPoint(hoverProbeOffset);
        Vector3 originBack = transform.TransformPoint(-hoverProbeOffset);
        Vector3 backToFront = originFront - originBack;
        didHitFront = Physics.SphereCast(originFront, hoverProbeRadius, Vector3.down, out hitFront, hoverHeight, layerMask);
        didHitBack = Physics.SphereCast(originBack, hoverProbeRadius, Vector3.down, out hitBack, hoverHeight, layerMask);

        if (didHitFront && didHitBack)
        {
            // ollaan kokonaan maassa
            isGrounded = true;

            Vector3 targetDir = backToFront + Vector3.down * (hitFront.distance - hitBack.distance);
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, angleAdjustStrength);

            // pystysuuntainen liikenopeus
            float heightDiff = hoverHeight - Mathf.Min(hitFront.distance, hitBack.distance);
            float targetVel = heightDiff * hoverSpeed;
            float velDiff = targetVel - rb.velocity.y;
            if (Mathf.Abs(velDiff) < maxHoverAcceleration)
            {
                rb.velocity = new Vector3(rb.velocity.x, targetVel, rb.velocity.z);
            }
            else
            {
                rb.velocity = rb.velocity + new Vector3(0, maxHoverAcceleration, 0);
            }
        }
        else if (didHitFront)
        {
            if (Vector3.Dot(fwd, rb.velocity) > 0)
            {
                // säädetään kulmaa vain jos mennään tasannetta kohti
                
            }
        }
        else if (didHitBack)
        {
            if (Vector3.Dot(fwd, rb.velocity) < 0)
            {
                
            }
        }
        else
        {
            // ollaan (ainakin osittain) ilmassa, käännetään alusta jonkin verran liikesuunnan mukaan

        }

        // kiihdyttäminen

        rb.velocity += vInput * thrust * fwd;

        

        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity *= maxSpeed / rb.velocity.magnitude;
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

    // Boxcastien debug-piirto
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        if (didHitFront)
        {
            Vector3 origin = transform.TransformPoint(hoverProbeOffset);
            Gizmos.DrawRay(origin, Vector3.down * hitFront.distance);
            Gizmos.DrawWireSphere(origin + Vector3.down * hitFront.distance, hoverProbeRadius);
        }

        if (didHitBack)
        {
            Vector3 origin = transform.TransformPoint(-hoverProbeOffset);
            Gizmos.DrawRay(origin, Vector3.down * hitBack.distance);
            Gizmos.DrawWireSphere(origin + Vector3.down * hitBack.distance, hoverProbeRadius);
        }
    }

    // päivitetään vastaavat vektorit kun editorissa muutetaan arvoja
    void OnValidate()
    {
        hoverProbeOffset = new Vector3(0, 0, hoverProbeDistance);
    }
}
