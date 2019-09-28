using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        player.GetComponent<Rigidbody>().AddExplosionForce(100f, player.transform.position, 5f);
        StartPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        DetectDeath();
    }

    void StartPlayer()
    {
        // Start player in random direction
    }

    void DetectDeath()
    {
        if (!playerScript.alive)
        {
            Debug.Log("Starting menu scene");
            SceneManager.LoadScene(0);
        }
    }
}
