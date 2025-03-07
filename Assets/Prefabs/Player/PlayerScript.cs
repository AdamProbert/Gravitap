﻿using UnityEngine;
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
    private Vector3 shrinkSpeed = new Vector3(.3f, .3f, .3f);
    private List<Vector3> prevPositions = new List<Vector3>();
    public bool isPaused = false;
    private float maxY; // Keep player stuck to floor

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
    public bool isTeleporting = false;
    public int lives = Parameters.PlayerLives;

    // Check on map Raycast
    private Ray mapRay;
    private RaycastHit mapHit;
    private string killLayer = "KillFloor";
    
    public void Ready()
    {
        GetComponent<Renderer>().enabled = true;
        tr.enabled = true;
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
        if (alive && !isPaused && !isTeleporting)
        {   
            if(!isFalling){
                CheckOffMap();    
            }
            if (isFalling && transform.localScale.x > 0.1)
            {
                transform.localScale = transform.localScale - shrinkSpeed * Time.deltaTime;
            }

            if (isPlaying && !isFalling)
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
                    StartCoroutine(KillPlayer(0f));
                }
                // Make sure trail is dope
                LifeChanges();
            }

        }
    }

    public void Teleport(Vector3 location)
    {
        isTeleporting = true;
        // Move player
        tr.time = -1;
        location.y = transform.position.y; // Retain player y axis
        transform.position = location;
        isTeleporting = false;
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
                StartCoroutine(KillPlayer(0));
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

        if (isPlaying)
        {
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

        }
    }


    private IEnumerator KillPlayer(float time)
    {
        yield return new WaitForSeconds(time);
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

    private void CheckOffMap(){
        Debug.DrawRay(transform.position, -transform.up*10f, Color.red, 50f);
          mapRay = new Ray(transform.position, -transform.up);
          if (Physics.Raycast(mapRay, out mapHit, 10f))
          {
              if(mapHit.transform.tag == killLayer)
              {
                  Debug.Log("Player is falling");
                  isFalling = true;
                  rb.constraints = RigidbodyConstraints.None;
              }
          }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "KillFloor")
        {
            source.Play();
            rb.AddForce(0, -500, 0);
            StartCoroutine(KillPlayer(1.5f));
        }
        if (collision.gameObject.tag == "Goal")
        {
            lives = Parameters.PlayerLives;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Body")
        {
            lives -= 1;
        }

        if(collision.gameObject.tag == "Projectile")
        {
          Vector3 dir = collision.contacts[0].point - transform.position;  
          dir = -dir.normalized;
          rb.AddForce(dir*Parameters.projectileSpeed);
        }
    }
}