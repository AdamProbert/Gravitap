using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private float startTime;
    private float rotationSpeed = 40f;
    private float spawnTime;

    private void Start()
    {
        startTime = Time.time;
        spawnTime = startTime + Random.Range(Parameters.minSpawnTime, Parameters.maxSpawnTime);     
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < spawnTime)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            rotationSpeed += 2f;
        }
        else
        {
            GetComponentInChildren<PlayerScript>().Spawned();
            Destroy(this.gameObject);
        }
    }
}
