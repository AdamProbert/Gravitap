using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScript : MonoBehaviour
{
    Collider col;
    private void Start()
    {
        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
        col = GetComponent<Collider>();
        col.enabled = false;
        StartCoroutine(WaitForSec(2f));
    }

    private void Update()
    {
        //rotates 180 degrees per second around z axis
        //transform.Rotate(0, 180 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GetComponent<Explosion>().Explode(Color.black);
            Destroy(other.gameObject);
        }
    }

    private IEnumerator WaitForSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        col.enabled = true;
    }
}
