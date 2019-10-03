using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour
{
    public GameObject goalPrefab;
    private GameObject currentGoal;
    public GameObject map;
    private float border;
    private BodyManager bm;
    public PlayerScript player;
    private ScoreManager sm;

    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.Find("BodyManager").GetComponent<BodyManager>();
        border = Parameters.border;
        sm = GetComponent<ScoreManager>();
    }


    public void GoalDeath(GameObject goal)
    {
        if (player.isPlaying)
        {
            Debug.Log("Registered goal death with manager and player is playing");
            SpawnGoal();
            sm.HitGoal(goal);
        }
    }

    // Spawns a new goal at random location on map
    public void SpawnGoal()
    {
        float zValue = map.transform.localScale.z/2 - border;
        float xValue = map.transform.localScale.x/2 - border;

        float spawnZ = Random.Range(-zValue, zValue);
        float spawnX = Random.Range(-xValue, xValue);
        bool goodSpawn = false;
        int spawnAttemptCount = 0;
        while (!goodSpawn)
        {
            if(spawnAttemptCount >= 3000)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().CantPlaceGoal();
                break;
            }
            spawnZ = Random.Range(-zValue, zValue);
            spawnX = Random.Range(-xValue, xValue);

            Collider[] colliders = Physics.OverlapSphere(new Vector3(spawnX, 0, spawnZ), goalPrefab.transform.localScale.x);
            goodSpawn = true;
            foreach(Collider c in colliders)
            {
                if(c.tag == "Goal" || c.tag == "Player" || c.tag == "Body" || c.tag == "DeadStar")
                {
                    goodSpawn = false;
                    break;
                }
            }
            spawnAttemptCount++;
        }

        Vector3 spawnPosition = new Vector3(spawnX, 1, spawnZ);
        currentGoal = Instantiate(goalPrefab, spawnPosition, Quaternion.identity);
        currentGoal.transform.parent = transform;
    }
}
