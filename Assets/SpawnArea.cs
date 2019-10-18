using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{

    public Vector3 size;

    private void OnDrawGizmos()
    {
        Color c = Color.green;
        c.a = .2f;
        Gizmos.color = c;
        Gizmos.DrawCube(transform.position, size);
    }
}
