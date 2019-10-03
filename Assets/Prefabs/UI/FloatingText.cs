using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    Quaternion initialRotation;

    void Start()
    {
        initialRotation = new Quaternion(0, 0, 0, 0);

    }

    void Upda()
    {
        transform.rotation = initialRotation;
    }
}
