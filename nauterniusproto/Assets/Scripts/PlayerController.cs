using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //public float acceleration;
    //public float brake;

    public delegate void MyDelegate();
    public event MyDelegate onDeath;

    public float hoverHeight;
    public float hoverForce;
    
    public float maxSpeed;
    public float speed;
    public float turnSpeed;

    public float rotationInput;
    public float speedInput;

    public bool superSpeed;
    public bool jumpAbility;
    public bool grounded;

    public Rigidbody rb;
    public Text uiText;

    public float test;

    public CheckPointManager cpManager;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        cpManager = GameObject.Find("Check Point Manager").GetComponent<CheckPointManager>();

        maxSpeed = 10000f;
        speed = maxSpeed;
        turnSpeed = 1.5f;

        hoverForce = 50f;
        hoverHeight = 2f;
	}
	
	void Update ()
    {
       rotationInput = Input.GetAxis("Horizontal");
       speedInput = Input.GetAxis("Vertical");

       transform.Rotate(0, rotationInput * turnSpeed, 0);
        // transform.Translate(0, 0, speedInput * maxSpeed);

        if (Input.GetButtonDown("Jump") && jumpAbility) Jump();
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
        }

        //Jos pelaajan alapuolella alle sadan metrin päässä ei ole mitään --> kuolema
        Ray deathRay = new Ray(transform.position, -transform.up);
        if (!Physics.Raycast(deathRay, 50f)) Die();

        //Yläviiston kääntyminen rampeissa onnistuisi ehkä jos laittaisi raycastit perään ja nokkaan tyyliin
        //Ray ray2 = new Ray(transform.position + (0,0,1.5f), -transform.up);
        //rb.AddForceAtPosition(forceUp, transform.position + (0,0,1.5f), ForceMode.Acceleration);

        //rb.AddRelativeTorque(0f, rotationInput * turnSpeed, 0f);
        rb.AddRelativeForce(0f, 0f, speedInput * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject colObject = collision.collider.gameObject;
        Debug.Log("Osui :" + colObject.name);
        
        if (colObject.tag == "wall") SuperSpeedOff();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "jumppowerup")
        {
            jumpAbility = true;
            Destroy(other.gameObject);
        }
    }

    public void SuperSpeedOn()
    {
        if (!superSpeed)
        {
            superSpeed = true;
            speed = 3 * maxSpeed;
        }
    }

    public void SuperSpeedOff()
    {
        if (superSpeed)
        {
            superSpeed = false;
            speed = maxSpeed;
        }
    }

    public void Jump()
    {
        Debug.Log("Hyppy");

        rb.AddForce(0f, 8000f, 0f, ForceMode.Impulse);
    }

    public void Die()
    {
        Debug.Log("Kuoli saatana");
        transform.position = cpManager.CurCheckPoint.spawnPoint;
        //transform.position = 
        //onDeath.Invoke();
    }
}
