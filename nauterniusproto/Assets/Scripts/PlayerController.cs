using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    
    public float hoverHeight;
    public float hoverForce;
    
    public float maxSpeed;
    public float speed;
    public float turnSpeed;

    public float rotationInput;
    public float speedInput;

    public bool hasSuperSpeed;
    public bool hasJumpAbility;
    public bool isGrounded;

    public Rigidbody rb;
    public CheckPointManager checkPointManager;

    //Käytetään aluksen kääntämisessä
    public Quaternion fromRotation;
    public Quaternion toRotation;
    public Vector3 targetNormal;
    public RaycastHit rcHit;
    public float weight;
    public float adjustSpeed;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        checkPointManager = GameObject.Find("Check Point Manager").GetComponent<CheckPointManager>();
        
        hoverForce = 50f;
        hoverHeight = 2f;

        maxSpeed = 10000f;
        speed = maxSpeed;
        turnSpeed = 1.5f;

        weight = 1;
        adjustSpeed = 1;
	}
	
	void Update ()
    {
       rotationInput = Input.GetAxis("Horizontal");
       speedInput = Input.GetAxis("Vertical");

       transform.Rotate(0, rotationInput * turnSpeed, 0);
        
       //Kääntää aluksen samaan kulmaan kuin alla oleva maa
       if (Physics.Raycast(transform.position, -Vector3.up, out rcHit))
       {
           fromRotation = transform.rotation;
           if (rcHit.distance < 3)
           {
               if (rcHit.normal == transform.up) return;
               if (rcHit.normal != targetNormal)
               {
                   targetNormal = rcHit.normal;
                   toRotation = Quaternion.FromToRotation(Vector3.up, rcHit.normal);
                   weight = 0;
               }
               if (weight <= 1)
               {
                   weight += Time.deltaTime * adjustSpeed;
                   toRotation = Quaternion.Euler(toRotation.eulerAngles.x, fromRotation.eulerAngles.y, toRotation.eulerAngles.z);
                   transform.rotation = Quaternion.Slerp(fromRotation, toRotation, weight);
               }
           }
       }
    }

    void FixedUpdate()
    {
        //Leijuminen - Nostaa autoa ylöspäin, kun se tippuu liian lähelle maata
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance); //Mitä lähempänä maata sitä isompi
            Vector3 forceUp = Vector3.up * proportionalHeight * hoverForce; //Nostaa lujempaa, mitä lähempänä maata

            rb.AddForce(forceUp, ForceMode.Acceleration);
            if (Input.GetButtonDown("Jump") && hasJumpAbility && hit.distance <= hoverHeight) Jump();
        }

        rb.AddRelativeForce(0f, 0f, speedInput * speed);
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
        Debug.Log("Hyppy");
        rb.AddForce(0f, 10000f, 0f, ForceMode.Impulse);
    }

    public void Die()
    {
        Debug.Log("Kuoli saatana");
        transform.position = checkPointManager.CurCheckPoint.spawnPoint;
        //transform.position = 
        //onDeath.Invoke();
    }
}
