using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float cubeSize = 0.2f;
    public int cubesInRow = 5;
    public GameObject explosionCube;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 100f;
    public float explosionRadius = 6f;
    public float explosionUpward = 0.8f;

    // Use this for initialization
    void Start()
    {

        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);

    }

    public void Explode(Color c)
    {
        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    CreatePiece(x, y, z, c);
                }
            }
        }

        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
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

    public void SpiralExplode(Color c)
    {
        float radius = 5f;
        int particles = 120;
        for (int i = 0; i<particles; i++)
        {
            float angle = i * Mathf.PI*2f/particles;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, .5f, Mathf.Sin(angle) * radius);

            CreatePiece(newPos.x, newPos.y, newPos.z, c);
            //Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            //foreach (Collider hit in colliders)
            //{
            //    Rigidbody rb = hit.GetComponent<Rigidbody>();
            //    if (rb != null)
            //        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            //}
        }
    }

    GameObject CreatePiece(float x, float y, float z, Color c)
    {
        
        // Create piece
        GameObject piece = GameObject.Instantiate(explosionCube);

        // Assign color
        piece.gameObject.GetComponent<Renderer>().material.SetColor("_Color", c);


        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        Destroy(piece, Random.Range(Parameters.goalExplosionMinLife, Parameters.goalExplosionMaxLife));

        return piece;
    }

}