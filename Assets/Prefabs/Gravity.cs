using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    const float GravConstant = 6.674f;
    public const float radius = 10f;
    private Rigidbody rb;
    private Rigidbody playerrb;

    private void Start()
    {
        playerrb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Attract();
    }

    void Attract()
    {
        Vector3 direction = rb.position - playerrb.position;
        float distance = direction.magnitude;

        if (distance <= radius)
        {
            float forceMagnitude = GravConstant * (rb.mass * playerrb.mass) / Mathf.Pow(distance, 2);
            Vector3 force = direction.normalized * forceMagnitude;

            playerrb.AddForce(force);        }
    }
}
