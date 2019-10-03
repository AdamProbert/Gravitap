using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{

    public GameObject player;        //Public variable to store a reference to the player game object


    private Vector3 offset;            //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        // Also freeze the y position
        Vector3 camPosition = player.transform.position + offset;
        camPosition.y = offset.y;
        transform.position = camPosition;
    }
}