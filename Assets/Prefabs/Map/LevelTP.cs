using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTP : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On trigger enter called on teleport");
        if (other.tag == "Player")
        {
            transform.GetComponentInParent<MapManager>().TransportPlayer();
        }
        else if(other.tag == "Body" || other.tag == "DeadStar" || other.tag == "Hazzard")
        {
            Destroy(other.gameObject);
        }
    }
}
