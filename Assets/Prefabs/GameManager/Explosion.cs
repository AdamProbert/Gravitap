using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public int cubesInRow = 5;
    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 100f;
    public float explosionRadius = 6f;
    public float explosionUpward = 0.8f;

    // Use this for initialization
    void Start()
    {

        //calculate pivot distance
        cubesPivotDistance = Parameters.DebrisSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);

    }

    public void Explode(Color c)
    {
        DebrisPooler debrispool = DebrisPooler.instance;
        float debrisSize = Parameters.DebrisSize;
        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    GameObject debris = debrispool.GetPooledObject();
                    if (debris != null)
                    {
                        debris.transform.position = transform.position + new Vector3(debrisSize * x, debrisSize * y, debrisSize * z) - cubesPivot;
                        debris.transform.rotation = transform.rotation;
                        debris.gameObject.GetComponent<Renderer>().material.SetColor("_Color", c);
                        debris.SetActive(true);
                    }
                }
            }
        }

        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            {
                //get rigidbody from collider object
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    //add explosion force to this body with given parameters
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
                }
            }
        }
    }
}