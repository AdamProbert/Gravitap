using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPiece : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        // Ignore collision with player
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
