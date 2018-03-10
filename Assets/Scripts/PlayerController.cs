using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Range(0, 10)]
    [SerializeField] private float hoverHeight;
    [Range(0, 10)]
    [SerializeField] private float hoverSpeed;
    [Range(0, 5)]
    [SerializeField] private float maxHoverAcceleration;
    [Range(0, 5)]
    [SerializeField] private float angleAdjustSpeed;
    [Range(0, 3)]
    [SerializeField] private float hoverProbeDistance; // leijumiseen käytettävien boxcastien välimatka keskipisteestä
    private Vector3 hoverProbeOffset;
    [Range(0, 3)]
    [SerializeField] private float hoverProbeRadius;

    [Range(0, 5)]
    [SerializeField] private float thrust; // kiihdytysteho
    [SerializeField] private float maxSpeed;
    private float trueMaxSpeed;
    [Range(2, 5)]
    [SerializeField] private float turnSpeed; // kuinka nopeasti alus kääntyy ohjauksesta
    [SerializeField] private float maxTurnAcceleration; // kuinka nopeasti aluksen nopeus voi muuttua kääntyessä (tämä aiheuttaa driftaamisen nopeissa vauhdeissa)

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
    private RaycastHit hitFront;
    private RaycastHit hitBack;
    private bool didHitFront;
    private bool didHitBack;
    private Vector3 toRotate;

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
        toRotate = new Vector3(0, 0, 0);

        // leijuminen

        didHitFront = Physics.SphereCast(transform.TransformPoint(hoverProbeOffset), hoverProbeRadius, Vector3.down, out hitFront, hoverHeight, layerMask);
        didHitBack = Physics.SphereCast(transform.TransformPoint(-hoverProbeOffset), hoverProbeRadius, Vector3.down, out hitBack, hoverHeight, layerMask);

        if (didHitFront && didHitBack)
        {
            // ollaan kokonaan maassa
            isGrounded = true;

            toRotate.x = (hitFront.distance - hitBack.distance) * angleAdjustSpeed;

            // pystysuuntainen liikenopeus
            float heightDiff = hoverHeight - Mathf.Min(hitFront.distance, hitBack.distance);
            float targetVel = heightDiff * hoverSpeed;
            float velDiff = targetVel - rb.velocity.y;
            if (velDiff < maxHoverAcceleration)
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
            // jos vain toinen pää on tason päällä, pitäisi kääntää jonkin verran ettei tule tiettyjä jumitilanteita (todo)
        }
        else if (didHitBack)
        {
            
        }
        else
        {
            // ollaan (ainakin osittain) ilmassa, käännetään alusta jonkin verran liikesuunnan mukaan
            isGrounded = false;


        }

        // kääntyminen

        toRotate.y = hInput * turnSpeed;


        // kiihdyttäminen

        rb.velocity += vInput * thrust * transform.forward;

        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity *= maxSpeed / rb.velocity.magnitude;
        }



        transform.Rotate(toRotate);
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

        //Gizmos.DrawRay(transform.TransformPoint(Vector3.zero), transform.forward * 20.0f);
    }

    // päivitetään vastaavat vektorit kun editorissa muutetaan arvoja
    void OnValidate()
    {
        hoverProbeOffset = new Vector3(0, 0, hoverProbeDistance);
    }
}
