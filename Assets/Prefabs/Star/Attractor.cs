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
    public GameObject killZone;
    public bool alive = true;
    ParticleSystem ps;
    public GameObject clickCollider;
    GameObject clickColliderGO;

    private void Start()
    {
        playerrb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        rb.AddTorque(transform.up * 25);
        position = new Vector3(rb.position.x, 0, rb.position.z);
        ps = GetComponentInChildren<ParticleSystem>();
        clickColliderGO = Instantiate(clickCollider, transform.position, transform.rotation);
        clickColliderGO.transform.parent = gameObject.transform;

    }

    private void FixedUpdate()
    {
        if (this.gameObject != null)
        {
            Attract();
        }
    }

    private void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }

    void Attract()
    {
        if (alive)
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
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(position, Vector3.up, radius);
        #endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { 
            //Deactivate Pulsing Circle, set color to black
            // turn of gravity, increase size and enable collisions, and explode
            foreach (Transform child in transform)
            {
                if (child.name == "PulsingCircle")
                    child.gameObject.SetActive(false);
            }
            if (alive)
            {
                GetComponent<Collider>().isTrigger = false;
                ps.Play();
                GetComponent<Explosion>().SpiralExplode(Color.black);
                GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                transform.localScale = transform.localScale * 1.5f;
                clickColliderGO.SetActive(false);
            }
            alive = false;
        }
    }
}
