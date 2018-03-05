using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    
    [SerializeField] private float hoverHeight = 2.0f;
    [SerializeField] private float hoverForce = 50.0f;

    [SerializeField] private float maxSpeed = 13000.0f;
    private float speed;
    [SerializeField] private float turnSpeed = 100.0f;
    [SerializeField] private float handling = 1.0f; //Vaikuttaa kuinka paljon input otetaan huomioon [0,1] //Edessä oleva kommentti ei näytä suomen kielen lauseelta

    private float rotationInput;
    private float speedInput;

    [SerializeField] private float jumpForce = 11000.0f;
    private float jumpTimer;
    [SerializeField] private float jumpWaitTime = 1.0f;

    private bool hasSuperSpeed;
    private bool hasJumpAbility;
    private bool isGrounded;

    private Rigidbody rb;
    [SerializeField] private CheckPointManager checkPointManager;

    //Käytetään aluksen kääntämisessä
    private Quaternion fromRotation;
    private Quaternion toRotation;
    private Vector3 targetNormal;
    private RaycastHit rcHit;
    private float weight;
    private float adjustSpeed;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        
        speed = maxSpeed;
        weight = 1;
        adjustSpeed = 1;
	}
	
	void Update ()
    {
       rotationInput = Input.GetAxis("Horizontal");
       speedInput = Input.GetAxis("Vertical");

       transform.Rotate(0, rotationInput * turnSpeed * Time.deltaTime, 0);

       jumpTimer += Time.deltaTime;
        
       //Kääntää aluksen samaan kulmaan kuin alla oleva maa
       if (Physics.Raycast(transform.position, -Vector3.up, out rcHit))
       {
           fromRotation = transform.rotation;

           if (rcHit.normal == transform.up) return;
           if (rcHit.normal != targetNormal)
           {
               weight = 0;
               targetNormal = rcHit.normal;
               toRotation = Quaternion.FromToRotation(Vector3.up, rcHit.normal);
           }
           if (weight <= 1)
           {
               weight += Time.deltaTime * adjustSpeed * (1 / rcHit.distance);
               if (weight > 1) weight = 1;
               toRotation = Quaternion.Euler(toRotation.eulerAngles.x, fromRotation.eulerAngles.y, toRotation.eulerAngles.z);
               transform.rotation = Quaternion.Slerp(fromRotation, toRotation, weight);
           }
           
       }
    }

    void FixedUpdate()
    {
        //Handling pienentäisi kaasutuksen vaikutusta ollessa ilmassa, tuntu paskalta ainakin näin simppelinä
        //handling = 0.5f;

        //Leijuminen - Nostaa autoa ylöspäin, kun se tippuu liian lähelle maata
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            //handling = 1;
            float proportionalHeight = (hoverHeight - hit.distance); //Mitä lähempänä maata sitä isompi
            Vector3 forceUp = Vector3.up * proportionalHeight * hoverForce; //Nostaa lujempaa, mitä lähempänä maata

            rb.AddForce(forceUp, ForceMode.Acceleration);
            if (Input.GetButtonDown("Jump") && hasJumpAbility && jumpTimer > jumpWaitTime) Jump();
        }

        rb.AddRelativeForce(0f, 0f, speedInput * speed);
        //rb.AddRelativeForce(0f, 0f, speedInput * speed * handling);
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
            speed = 3 * maxSpeed;
        }
    }

    public void SuperSpeedOff()
    {
        if (hasSuperSpeed)
        {
            hasSuperSpeed = false;
            speed = maxSpeed;
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
}
