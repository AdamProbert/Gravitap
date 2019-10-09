using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    Color color;
    AudioSource source;
    public bool alive = true;

    public void Setup()
    {
        source = GetComponent<AudioSource>();
    }

    public void SetColor(Color newcolor)
    {
        color = Parameters.colorList[Random.Range(0, Parameters.colorList.Count - 1)];
        GetComponent<Renderer>().material.SetColor("_Color", newcolor);
    }

    public void Rotate(int x, int y, int z)
    {
        transform.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
    }

    public void GoalDeath(bool spawnNew = true)
    {
        source.Play();
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Explosion>().Explode(color);
        alive = false;
        GetComponentInParent<GoalManager>().GoalDeath(this.gameObject);
        Destroy(this.gameObject, 1.5f);
    }
}
