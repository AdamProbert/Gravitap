using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    Color color;
    AudioSource source;
    public bool alive = true;

    private void Start()
    {
        // Set colour of goal to random colour from pallete
        color = Parameters.colorList[Random.Range(0, Parameters.colorList.Count - 1)];
        GetComponent<Renderer>().material.SetColor("_Color", color);
        source = GetComponent<AudioSource>();

    }

    void Update()
    {

        //rotates 50 degrees per second around z axis
        transform.Rotate(50 * Time.deltaTime, 50 * Time.deltaTime, 20 * Time.deltaTime); 

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
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
}
