using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(50 * Time.deltaTime, 50 * Time.deltaTime, 20 * Time.deltaTime); //rotates 50 degrees per second around z axis
    }
}
