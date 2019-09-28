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
    public bool alive = true;
    private bool isPlaying = false;
    private List<float> speedHistory = new List<float>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocityIncrease = Parameters.playerVelocityIncrease;
        minVelocity = Parameters.playerMinVelocity;
        maxVelocity = Parameters.playerMaxVelocity;
        speed = Parameters.playerSpeed;
        
    }

    private void Update()
    {
        float playerSpeedX = Mathf.Abs(rb.velocity.x);
        float playerSpeedZ = Mathf.Abs(rb.velocity.z);

        Debug.Log("PLayer speed x: " + playerSpeedX + "  PlayerSpeedZ: " + playerSpeedZ);
        if (playerSpeedX > Parameters.playerDeathSpeed && playerSpeedZ > Parameters.playerDeathSpeed)
        {
            isPlaying = true;
            Debug.Log("Player is playing");
        }
        

        if (isPlaying)
        {
            if (playerSpeedX < Parameters.playerDeathSpeed && playerSpeedZ < Parameters.playerDeathSpeed)
            {
                speedHistory.Add(playerSpeedX);
            }
            else
            {
                speedHistory.Clear();
            }

            if(speedHistory.Count > 100)
            {
                alive = false;
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
        else if(v.sqrMagnitude > sqrMinVelocity)
        {
            rb.velocity = v.normalized * minVelocity;
        }

        // Ensure player doesn't come off surface
        if(transform.position.y > 0)
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "KillFloor")
        {
            Debug.Log("Player died");
            alive = false;
            GetComponent<Renderer>().enabled = false;
        }
    }
}