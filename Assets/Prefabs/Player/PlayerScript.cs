using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    private float speed;
    private Rigidbody rb;
    private float maxVelocity;
    private float minVelocity;
    private float velocityIncrease;
    private float sqrMaxVelocity;
    private float sqrMinVelocity;
    public GameObject debris;
    private AudioSource source;

    // States
    private bool isFalling = false;
    private List<float> speedHistory = new List<float>();
    public bool alive = true;
    private bool isPlaying = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocityIncrease = Parameters.playerVelocityIncrease;
        minVelocity = Parameters.playerMinVelocity;
        maxVelocity = Parameters.playerMaxVelocity;
        speed = Parameters.playerSpeed;

        // Audio
        source = GetComponent<AudioSource>();

        // Ignore collisions with debris
        Physics.IgnoreLayerCollision(9, 8);

    }

    private void Update()
    {
        float playerSpeedX = Mathf.Abs(rb.velocity.x);
        float playerSpeedZ = Mathf.Abs(rb.velocity.z);

        if (!isPlaying && playerSpeedX > Parameters.playerDeathSpeed && playerSpeedZ > Parameters.playerDeathSpeed)
        {
            isPlaying = true;
        }


        if (isPlaying)
        {

            // Check player is moving
            if (playerSpeedX < Parameters.playerDeathSpeed && playerSpeedZ < Parameters.playerDeathSpeed)
            {
                speedHistory.Add(playerSpeedX);
            }
            else
            {
                speedHistory.Clear();
            }

            if (speedHistory.Count > 100)
            {
                Debug.Log("Player moved to slow");
                KillPlayer();
            }
        }
    }

    void SetMaxVelocity(float maxVelocity)
    {
        sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    void SetMinVelocity(float minVelocity)
    {
        sqrMinVelocity = minVelocity * minVelocity;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);

        Vector3 v = rb.velocity;
        if (v.sqrMagnitude > sqrMaxVelocity)
        {
            rb.velocity = v.normalized * maxVelocity;
        }
        // Need to set min velocity for player too.. keep him moving yo!
        else if (v.sqrMagnitude > sqrMinVelocity)
        {
            rb.velocity = v.normalized * minVelocity;
        }

        // Ensure player doesn't come off surface
        if (transform.position.y > 0)
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void KillPlayer()
    { 
        Debug.Log("Player died");
        alive = false;
        GetComponent<Renderer>().enabled = false;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "KillFloor")
        {
            KillPlayer();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (
            !isFalling &&
            collision.transform.tag == "World" && 
            !Physics.Raycast(transform.position, Vector3.down, 5)
            )
        {
            Debug.Log("Player falling");
            isFalling = true;
            rb.AddForce(0, -2000, 0);
            source.Play();
        }    
    }

    //Detect when there is a collision
    void OnCollisionStay(Collision collide)
    {
        //Output the name of the GameObject you collide with
        //Debug.Log("I hit the GameObject : " + collide.gameObject.name);
    }
}