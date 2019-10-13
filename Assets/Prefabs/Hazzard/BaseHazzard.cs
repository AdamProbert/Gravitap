using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHazzard : MonoBehaviour
{
    AudioSource source;
    public bool alive = true;
    private int minScore;
    public int spawnFrequency;

    public void Setup(int myMinScore, int mySpawnFrequency)
    {
        minScore = myMinScore;
        spawnFrequency = mySpawnFrequency;
        source = GetComponent<AudioSource>();
    }

    public AudioSource GetSource()
    {
        return source;
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
        source.Play();
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Explosion>().Explode(Parameters.black);
        alive = false;
        Destroy(this.gameObject, 1.5f);
    }
}
