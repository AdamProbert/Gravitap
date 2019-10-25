using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explode;
    private bool hasHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(!hasHit){
            GetComponent<TrailRenderer>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Instantiate(explode, transform.position, Quaternion.identity);
            hasHit = true;
            Destroy(this.gameObject, 2f);            
        }
    }
}
