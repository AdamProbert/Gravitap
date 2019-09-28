using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{

    public GameObject star;
    private Queue<GameObject> stars = new Queue<GameObject>();

    private void Update()
    {
        CleanStarQueue();

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.gameObject.tag == "World")
                {
                    Vector3 spawnBody = new Vector3(hit.point.x, 0, hit.point.z);
                    GameObject newStar = Instantiate(star, spawnBody, Quaternion.identity);
                    this.stars.Enqueue(newStar);
                }
                else if (hit.collider.gameObject.tag == "BodyClickTrigger")
                {
                    if (hit.collider.gameObject.GetComponentInParent<Attractor>().alive == true)
                        Destroy(hit.collider.transform.parent.gameObject);
                }
            }
            
        }
    }

    private void CleanStarQueue()
    {
        GameObject oldStar = null;
        // Cycle through queue, remove nulls or dead. Enqueue valid objects
        if (this.stars.Count > Parameters.maxStars)
        {
            int count = this.stars.Count;
            for (int i = 0; i < count; i++)
            {
                oldStar = this.stars.Dequeue();
                if (oldStar != null && oldStar.GetComponent<Attractor>().alive == true)
                {
                    stars.Enqueue(oldStar);
                }
            }          
            
        }
        if (this.stars.Count > Parameters.maxStars)
        {
            oldStar = this.stars.Dequeue();
            Destroy(oldStar);
        }
    }
}
