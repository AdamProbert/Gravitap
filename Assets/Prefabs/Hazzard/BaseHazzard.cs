using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHazzard : MonoBehaviour
{
    public bool alive = true;
    public abstract int minScore { get; }
    public abstract int spawnFrequency { get; }
    
    public int GetSpawnFrequency()
    {
        return spawnFrequency;
    }

    public bool IsInScoreRange(int score)
    {
        if (score > minScore)
        {
            return true;
        }
        return false;
    }

    public void Rotate(int x, int y, int z)
    {
        transform.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
    }

    public void HazzardDeath()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Explosion>().Explode(Parameters.black);
        alive = false;
        Destroy(this.gameObject, 1.5f);
    }
}
