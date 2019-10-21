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

    public AudioSource GetSource()
    {
        return source;
    }

    public void SetColor(Color newcolor)
    {
        color = newcolor;
        
        // Change material colour
        GetComponent<Renderer>().material.SetColor("_Color", newcolor);

        // Change particle system colour
        GradientColorKey[] colourGradients = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        Gradient gradient = new Gradient();
        colourGradients[0].color = Color.white;
        colourGradients[0].time = 0.0f;
        colourGradients[1].color = color;
        colourGradients[1].time = 0.5f;
        alphaKeys[0].alpha = 1.5f;
        alphaKeys[0].time = 1.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 0.5f;
        gradient.SetKeys(colourGradients, alphaKeys);

        ParticleSystem ps = transform.Find("GoalGlow").Find("OrbGlow").GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.color = gradient;

    }

    public void Rotate(int x, int y, int z)
    {
        transform.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
    }

    public void GoalDeath()
    {
        source.Play();
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Explosion>().Explode(color);
        transform.Find("GoalGlow").gameObject.SetActive(false);
        alive = false;
        GetComponentInParent<GoalManager>().GoalDeath(this.gameObject);
        Destroy(this.gameObject, 1.5f);
    }
}
