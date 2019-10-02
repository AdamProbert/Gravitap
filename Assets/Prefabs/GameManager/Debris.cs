
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    private void OnEnable()
    {
        float lifetime = Random.Range(Parameters.goalExplosionMinLife, Parameters.goalExplosionMaxLife);
        Invoke("Deactivate", lifetime);
    }

    void Deactivate()
    {
        
        this.gameObject.SetActive(false);
    }
}