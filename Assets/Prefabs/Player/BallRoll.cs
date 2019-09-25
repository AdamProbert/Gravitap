using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BallRoll : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    public float maxVelocity;
    public float minVelocity;
    private float sqrMaxVelocity;
    private float sqrMinVelocity;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetMaxVelocity(maxVelocity);
    }

    void SetMaxVelocity(float maxVelocity)
    {
        this.maxVelocity = maxVelocity;
        sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    void SetMinVelocity(float minVelocity)
    {
        this.minVelocity = minVelocity;
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
    }
}