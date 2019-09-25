using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Attractor : MonoBehaviour
{

    public float GravConstant = 6.674f;
    public float radius = 10f;
    private Rigidbody rb;
    private Rigidbody playerrb;
    private Vector3 position;
    public bool debug;
    public float clampForce;
    public GameObject explosion;

    private void Start()
    {
        playerrb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        rb.AddTorque(transform.up * 25);
        position = new Vector3(rb.position.x, 0, rb.position.z);

    }

    private void FixedUpdate()
    {
        Attract();
    }

    void Attract()
    {
        Vector3 direction = position - playerrb.position;
        float distance = direction.magnitude;

        if (distance <= radius)
        {
            //float forceMagnitude = GravConstant * (rb.mass * playerrb.mass) / Mathf.Pow(distance, 2);
            float forceMagnitude = GravConstant * (rb.mass * playerrb.mass);
            Vector3 force = direction.normalized * forceMagnitude;
            force.x = Mathf.Clamp(force.x, -clampForce, clampForce);
            force.z = Mathf.Clamp(force.z, -clampForce, clampForce);

            playerrb.AddForce(force);
            //Debug.Log("Add force to player: " + force);
            if (debug)
            {
                Debug.DrawLine(position, playerrb.position, Color.green, 2.5f);
            }


        }
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(position, Vector3.up, radius);
        #endif
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Removing game object, playing explosion");
            //ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            //ps.Play();
            Destroy(this.gameObject);
        }
    }
}
