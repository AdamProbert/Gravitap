using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{

    public Vector3 size;
    public bool drawSpawn;

    private void OnDrawGizmos()
    {
        if (drawSpawn)
        {
            Color c = Color.green;
            c.a = .2f;
            Gizmos.color = c;
            Gizmos.DrawCube(transform.position, size);
        }
    }
}
