using System.Collections;
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
    [SerializeField] private float hoverAcceleration;
    [Range(0, 5)]
    [SerializeField] private float pitchAdjustSpeedGrounded;
    [Range(0, 3)]
    [SerializeField] private float pitchAdjustSpeedAir;
    [Range(0, 60)]
    [SerializeField] private float maxAirPitch;
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

    [Range(0, 1)]
    [SerializeField] private float onGroundDrag;
    [Range(0, 1)]
    [SerializeField] private float inAirDrag;
    
    [Header("Other")]
    [SerializeField] private float jumpForce;
    private int jumpTimer;
    [SerializeField] private int jumpWaitTime; // hypyn cooldown fixedDeltaTime-intervalleina (frameina)
    [SerializeField] private CheckPointManager checkPointManager;
    [SerializeField]
    private float fallMulti;
    [SerializeField]
    private float jumpMulti;

    public delegate void FilterChange(bool filterStatus);
    public event FilterChange OnFilterChanged;

    private bool hasSuperSpeed;
    private bool hasJumpAbility;
    private bool isGrounded;
    private Rigidbody rb;
    public bool HasFilter { get; set; }
    private bool filterOn;
    public bool FilterOn
    {
        get { return filterOn; }
        set
        {
            if (filterOn != value)
            {
                filterOn = value;
                OnFilterChanged.Invoke(value);
            }
        }
    }
    
    // update-loopissa käytettäviä muuttujia
    private float hInput;
    private float vInput;
    private RaycastHit hitFront;
    private RaycastHit hitBack;
    private bool didHitFront;
    private bool didHitBack;
    private Vector3 groundNormal;
    float targetRoll;
    float currentRoll;
    float targetPitch;
    float currentPitch;

    private const int layerMask = 1 << 8; // maski estää osumat muihin kuin terrain-layerin objekteihin
    

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        hoverProbeOffset = new Vector3(0, 0, hoverProbeDistance);
        trueMaxSpeed = maxSpeed;
        jumpTimer = 0;
        hasSuperSpeed = false;
        OnFilterChanged.Invoke(false);
        hasJumpAbility = false;
        isGrounded = false;
	}

    void FixedUpdate()
    {
        jumpTimer += 1;

        hInput = Input.GetAxis("Steer");
        vInput = Input.GetAxis("Forward") - Input.GetAxis("Reverse");
        isGrounded = false;

        // leijuminen

        didHitFront = Physics.SphereCast(transform.TransformPoint(hoverProbeOffset), hoverProbeRadius, Vector3.down, out hitFront, hoverHeight + groundFollowHeight, layerMask);
        didHitBack = Physics.SphereCast(transform.TransformPoint(-hoverProbeOffset), hoverProbeRadius, Vector3.down, out hitBack, hoverHeight + groundFollowHeight, layerMask);

        if (didHitFront && didHitBack)
        {
            // ollaan kokonaan maassa
            isGrounded = true;

            transform.Rotate((hitFront.distance - hitBack.distance) * pitchAdjustSpeedGrounded, 0, 0, Space.Self);

            // pystysuuntainen liikenopeus
            float minDistance = Mathf.Min(hitFront.distance, hitBack.distance);
            if (minDistance <= hoverHeight)
            {
                groundNormal = (hitFront.normal + hitBack.normal + Vector3.up).normalized;

                float targetVel = (hoverHeight - minDistance) * maxHoverSpeed;
                float velDiff = targetVel - Vector3.Dot(rb.velocity, groundNormal);

                if (velDiff > 0) rb.velocity += Mathf.Min(velDiff, hoverAcceleration) * groundNormal;
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
            // ollaan ilmassa, käännetään alusta jonkin verran liikesuunnan mukaan

            currentPitch = transform.localEulerAngles.x;
            if (currentPitch > 180.0f) currentPitch -= 360.0f;

            targetPitch = Vector3.SignedAngle(new Vector3(transform.forward.x, 0, transform.forward.z), rb.velocity, transform.right);
            if (targetPitch > 90.0f) targetPitch -= 180.0f; // liikutaan takaperin
            else if (targetPitch < -90.0f) targetPitch += 180.0f;
            targetPitch = Mathf.Clamp(targetPitch, -maxAirPitch, maxAirPitch);

            float diff = targetPitch - currentPitch;
            if (Mathf.Abs(diff) > pitchAdjustSpeedAir)
            {
                transform.Rotate(Mathf.Sign(diff) * pitchAdjustSpeedAir, 0, 0, Space.Self);
            }
            else
            {
                transform.Rotate(diff, 0, 0, Space.Self);
            }
        }

        // kallistus

        currentRoll = transform.localEulerAngles.z;
        if (currentRoll > 180.0f) currentRoll -= 360.0f;
        targetRoll = -hInput * rollAngle;
        transform.Rotate(0, 0, (targetRoll - currentRoll) * rollSpeed, Space.Self);

        // kiihdyttäminen ja kääntyminen

        if (isGrounded)
        {
            rb.velocity = Vector3.ProjectOnPlane(rb.velocity, new Vector3(transform.right.x, 0, transform.right.z)) + transform.forward * vInput * thrust;
            transform.Rotate(0, hInput * turnSpeed, 0, Space.World);
            rb.drag = onGroundDrag;
        }
        else
        {
            rb.velocity = Vector3.ProjectOnPlane(rb.velocity, new Vector3(transform.right.x, 0, transform.right.z)) + Vector3.ProjectOnPlane(transform.forward * vInput * thrust * airControl, Vector3.up);
            transform.Rotate(0, hInput * turnSpeed * airControl, 0, Space.World);
            rb.drag = inAirDrag;
        }


        if (rb.velocity.z * rb.velocity.z + rb.velocity.x * rb.velocity.x > trueMaxSpeed * trueMaxSpeed)
        {
            rb.velocity *= trueMaxSpeed / rb.velocity.magnitude;
        }

        //todo
        if (isGrounded && Input.GetButtonDown("Jump") && jumpTimer > jumpWaitTime && hasJumpAbility)
        {
            Jump();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && rb.velocity.magnitude < 5 && HasFilter) FilterOn = true;
        if (rb.velocity.magnitude > 5 && FilterOn) FilterOn = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject colObject = collision.collider.gameObject;
        
        if (colObject.layer == 8) SuperSpeedOff();
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

        rb.AddRelativeForce(rb.velocity + Vector3.up * jumpForce, ForceMode.Impulse);
        if(rb.velocity.y < 0 ) rb.velocity += Vector3.up * Physics.gravity.y * (fallMulti - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) rb.velocity += Vector3.up * Physics.gravity.y * (jumpMulti - 1) * Time.deltaTime;
        jumpTimer = 0;
    }

    public void Die()
    {
        Debug.Log("Kuoli saatana");
        transform.position = checkPointManager.CurCheckPoint.SpawnPoint;
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
