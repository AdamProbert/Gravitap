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
    private AudioSource source;

    // States
    private bool isFalling = false;

    public bool alive = true;
    public bool isPlaying = false; // Used for handling death states triggering before start
    private List<Vector3> prevPositions = new List<Vector3>();

    public void Spawned()
    {
        isPlaying = true;
        transform.parent = null;
        rb.velocity = (transform.TransformDirection(Vector3.back)* maxVelocity);
    }

    void Start()
    {
        prevPositions.Add(transform.position);
        rb = GetComponent<Rigidbody>();
        velocityIncrease = Parameters.playerVelocityIncrease;
        minVelocity = Parameters.playerMinVelocity;
        maxVelocity = Parameters.playerMaxVelocity;
        speed = Parameters.playerSpeed;

        // Audio
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {

        float playerSpeedX = Mathf.Abs(rb.velocity.x);
        float playerSpeedZ = Mathf.Abs(rb.velocity.z);


        if (isPlaying)
        {

            int prevPositionsCount = prevPositions.Count;

            // Check player is moving
            if (prevPositions[prevPositionsCount - 1] == transform.position)
            {
                
                if (prevPositionsCount > 1)
                {
                    Debug.Log("Player hasn't moved for " + prevPositions.Count + " frames");
                }
                prevPositions.Add(transform.position);

            }
            else
            {
                prevPositions.Clear();
                prevPositions.Add(transform.position);
            }

            if (prevPositions.Count > 100)
            {
                Debug.Log("Player stopped moving");
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
        float movehorizontal = Input.GetAxis("Horizontal");
        float movevertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(movehorizontal, 0.0f, movevertical);

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
}