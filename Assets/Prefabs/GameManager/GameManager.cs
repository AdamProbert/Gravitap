using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private GameObject player;
    private PlayerScript playerScript;
    private BodyManager bm;


    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.Find("BodyManager").GetComponent<BodyManager>();
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
    
    public void CantPlaceGoal()
    {
        Debug.Log("Cant place goal, quiting");
        EndGame();
    }

    void StartPlayer()
    {
        // Start player in random direction
    }

    void DetectDeath()
    {
        if (!playerScript.alive)
        {
            EndGame();
        }

        if (bm.getKillCount() >= Parameters.killCount)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
