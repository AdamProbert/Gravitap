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
    private Vector3 shrinkSpeed = new Vector3(.5f, .5f, .5f);
    private List<Vector3> prevPositions = new List<Vector3>();
    public bool isPaused = false;

    // Trail renderer
    private TrailRenderer tr;
    private Gradient originalTrailGradient;
    GradientColorKey[] colour2 = new GradientColorKey[2]; // Trail colours with 2 lives
    GradientAlphaKey[] alphaKey2 = new GradientAlphaKey[2];
    GradientColorKey[] colour1 = new GradientColorKey[1]; // Trail colours with 1 life
    GradientAlphaKey[] alphaKey1 = new GradientAlphaKey[1];
    private Gradient gradient2 = new Gradient();
    private Gradient gradient1 = new Gradient();


    // States
    public bool isFalling = false;
    public bool alive = true;
    public bool isPlaying = false; // Used for handling death states triggering before start
    public int lives = Parameters.PlayerLives;
    
    public void Ready()
    {
        GetComponent<Renderer>().enabled = true;
        transform.Find("TargetIndicator").gameObject.SetActive(true);
    }

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

        // Trail
        tr = GetComponent<TrailRenderer>();
        originalTrailGradient = tr.colorGradient;

        // 2 Lives remaining
        colour2[0].color = Parameters.green;
        colour2[0].time = 0.0f;
        colour2[1].color = Parameters.red;
        colour2[1].time = 0.5f;
        alphaKey2[0].alpha = 1.0f;
        alphaKey2[0].time = 1.0f;
        alphaKey2[1].alpha = 1.0f;
        alphaKey2[1].time = 1.0f;
        gradient2.SetKeys(colour2, alphaKey2);

        // 1 Life remaining
        colour1[0].color = Parameters.red;
        colour1[0].time = 0.0f;
        alphaKey1[0].alpha = 1.0f;
        alphaKey1[0].time = 0.8f;
        gradient1.SetKeys(colour1, alphaKey1);
    }

    private void Update()
    {
        if (alive)
        {
            if (isFalling)
            {
                transform.localScale = transform.localScale - shrinkSpeed * Time.deltaTime;
            }

            if (isPlaying)
            {

                float playerSpeedX = Mathf.Abs(rb.velocity.x);
                float playerSpeedZ = Mathf.Abs(rb.velocity.z);

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
    }

    void LifeChanges()
    {
        switch (lives)
        {
            case 3:
                GetComponent<Renderer>().material.SetColor("_Color", Parameters.blue);
                tr.time = 5;
                tr.colorGradient = originalTrailGradient;
                break;
            case 2:
                GetComponent<Renderer>().material.SetColor("_Color", Parameters.green);
                tr.time = 2;
                tr.colorGradient = gradient2;

                break;
            case 1:
                GetComponent<Renderer>().material.SetColor("_Color", Parameters.red);
                tr.time = 1;
                tr.colorGradient = gradient1;
                break;
            case 0:
                KillPlayer();
                break;
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
        //float movehorizontal = Input.GetAxis("Horizontal");
        //float movevertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(movehorizontal, 0.0f, movevertical);

        //rb.AddForce(movement * speed);

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

    public void TogglePause()
    {
        if(!isPaused && isPlaying && !isFalling)
        {
            isPaused = true;
        }
        else if (isPaused)
        {
            isPaused = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "KillFloor")
        {
            KillPlayer();
        }
        if (collision.gameObject.tag == "Goal")
        {
            lives = Parameters.PlayerLives;
            LifeChanges();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Body")
        {
            lives -= 1;
            LifeChanges();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (
            !isFalling &&
            collision.transform.tag == "World" && 
            !Physics.Raycast(transform.position, Vector3.down*20f, 5)
            )
        {
            Debug.Log("Player falling");
            isFalling = true;
            rb.AddForce(0, -2000, 0);
            source.Play();
        }
    }
}