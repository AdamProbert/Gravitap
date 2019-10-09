using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    Vector3 targetPostition = Vector3.zero;

    void Update()
    {
        FindTarget();
        transform.LookAt(targetPostition);
        transform.Rotate(0, -90, 0); // rotate back 90 cause weird shiz, trust me. Leave it as is.

    }

    void FindTarget()
    {
        {
            GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
            Debug.Log("goals: " + goals);
            foreach(GameObject g in goals)
            {
                Debug.Log("Goal:" + g);
                if (g.GetComponent<Goal>().alive)
                    targetPostition = g.transform.position;
            }
            
        }
    }
}
