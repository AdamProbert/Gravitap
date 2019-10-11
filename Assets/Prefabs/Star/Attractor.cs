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
    private PlayerScript playerScript;
    private Vector3 position;
    public float clampForce;
    public bool alive = true;
    ParticleSystem ps;
    public GameObject clickCollider;
    GameObject clickColliderGO;
    private AudioSource source;
    public GameObject deadStar;
    public bool playerTouch = false; // Flag for if player has interacted with this stars gravity
    private ScoreManager sm;

    private void Start()
    {     
        sm = GetComponentInParent<ScoreManager>();
        playerrb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        rb = GetComponent<Rigidbody>();
        rb.AddTorque(transform.up * 25);
        position = new Vector3(rb.position.x, rb.position.y-GetComponent<Renderer>().bounds.size.y/2, rb.position.z);
        ps = GetComponentInChildren<ParticleSystem>();
        clickColliderGO = Instantiate(clickCollider, transform.position, transform.rotation);
        clickColliderGO.transform.parent = gameObject.transform;
        clickColliderGO.transform.Translate(0, 1.3f, 0);
        clickColliderGO.transform.Rotate(270, 0, 0);
        source = GetComponent<AudioSource>();
        deadStar = Instantiate(deadStar, transform.position, transform.rotation, transform.parent);
        deadStar.SetActive(false);   
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
        transform.Rotate(0, 150 * Time.deltaTime, 0);
    }

    void Attract()
    {
        if (alive)
        {
            Vector3 direction = position - playerrb.position;
            float distance = direction.magnitude;

            // Inside gravity
            if (distance <= radius && !playerScript.isFalling)
            {
                //float forceMagnitude = GravConstant * (rb.mass * playerrb.mass) / Mathf.Pow(distance, 2);
                float forceMagnitude = GravConstant * (rb.mass * playerrb.mass);
                Vector3 force = direction.normalized * forceMagnitude;
                force.x = Mathf.Clamp(force.x, -clampForce, clampForce);
                force.z = Mathf.Clamp(force.z, -clampForce, clampForce);
                playerrb.AddForce(force);

                if (!playerTouch)
                {
                    ps.Play();
                    playerTouch = true;
                    sm.TriggeredStar(this.gameObject);
                }
            }

            // Outside of gravity
            else if (playerTouch)
            {
                playerTouch = false;
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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

    public void RemoveSelf()
    {
        Destroy(deadStar);
        Destroy(this.gameObject);
    }

    public void DestroySelf()
    {
        // Allert body manager
        GetComponentInParent<BodyManager>().StarDeath();

        //Deactivate Pulsing Circle, set color to black
        // turn of gravity, increase size and enable collisions, and explode
        GetComponent<Collider>().enabled = false;
        foreach (Transform child in transform)
        {
            if (child.name == "PulsingCircle")
                child.gameObject.SetActive(false);
        }
        GetComponent<Renderer>().enabled = false;
        clickColliderGO.SetActive(false);
        ps.Play();
        source.Play();
        GetComponent<Explosion>().Explode(Parameters.red);
        //GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        //transform.localScale = transform.localScale * 1.5f;
        //clickColliderGO.SetActive(false);

        deadStar.SetActive(true);
        alive = false;
        Destroy(this.gameObject, 1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (alive && other.gameObject.tag == "Player")
        {
            DestroySelf();
        }
    }
}
