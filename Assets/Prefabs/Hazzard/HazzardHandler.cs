using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazzardHandler : MonoBehaviour
{
    public GameObject map;
    private float border;
    private GameObject currentHazzard;
    public BaseHazzard[] hazzards;
    private ScoreManager sm;
    private int nextHazzardScore;
    private int hazzardScoreIncrement;

    void Start()
    {

        border = Parameters.border;
        sm = GetComponent<ScoreManager>();
        sm.OnScoreChange += ScoreChangeHandler;
        nextHazzardScore = Parameters.initialHazzardScore;
        hazzardScoreIncrement = Parameters.hazzardScoreIncrement;
    }

    void ScoreChangeHandler(int newscore)
    {
        Debug.Log("Score change hanlder called with: " + newscore);
        if (newscore > nextHazzardScore)
        {
            SpawnHazzard(newscore);
            nextHazzardScore += hazzardScoreIncrement;
        }
    }

    // Loop through the hazzards, creating a sublist of those applicible to the current core.
    // Then add them multipe times based on hazzards spawn frequency
    // Finally select one at randoom
    private BaseHazzard SelectHazzard(int score)
    {
        List <BaseHazzard> sublist = new List<BaseHazzard>();
        foreach(BaseHazzard h in hazzards)
        {
            if (h.IsInScoreRange(score))
            {
                for (int i = 0; i <= h.spawnFrequency; i++)
                {
                    sublist.Add(h);
                }       
            }
        }

        if(sublist.Count > 0)
        {
            return sublist[Random.Range(0, sublist.Count)];
        }

        else
        {
            return null;
        }

    }

    // Spawns a new goal at random location on map
    public void SpawnHazzard(int score)
    {
        BaseHazzard h = SelectHazzard(score);
        if (h)
        {
            currentHazzard = Instantiate(h.gameObject, GetSpawnPoint(h.gameObject), Quaternion.identity);
            currentHazzard.transform.parent = transform;
        }
    }

    private Vector3 GetSpawnPoint(GameObject h)
    {
        float zValue = map.transform.localScale.z / 2 - border;
        float xValue = map.transform.localScale.x / 2 - border;

        float spawnZ = Random.Range(-zValue, zValue);
        float spawnX = Random.Range(-xValue, xValue);
        bool goodSpawn = false;
        int spawnAttemptCount = 0;
        while (!goodSpawn)
        {
            if (spawnAttemptCount >= 3000)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().CantPlaceGoal();
                break;
            }
            spawnZ = Random.Range(-zValue, zValue);
            spawnX = Random.Range(-xValue, xValue);

            Collider[] colliders = Physics.OverlapSphere(new Vector3(spawnX, 0, spawnZ), h.transform.localScale.x);
            goodSpawn = true;
            foreach (Collider c in colliders)
            {
                if (c.tag == "Goal" || c.tag == "Player" || c.tag == "Body" || c.tag == "DeadStar" || c.tag == "Hazzard")
                {
                    goodSpawn = false;
                    break;
                }
            }
            spawnAttemptCount++;
        }

        Vector3 spawnPosition = new Vector3(spawnX, 1, spawnZ);

        return spawnPosition;
    }

}
