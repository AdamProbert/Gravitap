using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    Color color;
    AudioSource source;
    ParticleSystem ps;
    public bool alive = true;

    List<Color> colorList = new List<Color>()
     {
         new Color32(85, 115, 204, 1),
         new Color32(110, 53, 155, 1),
         new Color32(41, 188, 90, 1),
         new Color32(204, 64, 0, 1),
     };

    private void Start()
    {
        // Set colour of goal to random colour from pallete
        color = colorList[Random.Range(0, colorList.Count - 1)];
        Debug.Log("selected color " + color.ToString());
        GetComponent<Renderer>().material.SetColor("_Color", color);
        source = GetComponent<AudioSource>();
        ps = transform.Find("GoalExplosion").GetComponent<ParticleSystem>();

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
            GetComponent<Renderer>().enabled = false;
            ps.Play();
            Debug.Log("Exploding with color" + color.ToString());
            GetComponent<Explosion>().Explode(color);
            alive = false;
            Destroy(this.gameObject, 1f);
        }
    }
}
