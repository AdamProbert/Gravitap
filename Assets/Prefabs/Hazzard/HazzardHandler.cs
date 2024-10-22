﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazzardHandler : MonoBehaviour
{
    public GameObject map;
    private List<GameObject> currentHazzards;
    public BaseHazzard[] hazzards;
    private ScoreManager sm;
    private int nextHazzardScore;
    private int hazzardScoreIncrement;
    private MapManager mm;

    private BodyManager bm;

    void Start()
    {
        currentHazzards = new List<GameObject>();
        sm = GetComponent<ScoreManager>();
        sm.OnScoreChange += ScoreChangeHandler;
        mm = GameObject.Find("MapManager").GetComponent<MapManager>();
        mm.OnMapChange += MapChangeHandler;
        nextHazzardScore = Parameters.initialHazzardScore;
        hazzardScoreIncrement = Parameters.hazzardScoreIncrement;

        bm = GetComponent<BodyManager>();
    }

    void ScoreChangeHandler(int newscore)
    {
        if (newscore > nextHazzardScore)
        {
            SpawnHazzard(newscore);
            nextHazzardScore += hazzardScoreIncrement;
        }
    }

    void MapChangeHandler(GameObject newmap)
    {
        map = newmap;
        DestroyAllHazzards();
    }

    void DestroyAllHazzards()
    {
        foreach(GameObject h in currentHazzards)
        {
            Destroy(h);
        }
    }

    public int getHazzardCount()
    {
        return currentHazzards.Count;
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
                for (int i = 0; i < h.GetSpawnFrequency(); i++)
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
            GameObject newHazzard = Instantiate(h.gameObject, GetSpawnPoint(h.gameObject), Quaternion.identity);
            newHazzard.transform.parent = transform;
            currentHazzards.Add(newHazzard);
        }
    }

    public Vector3 GetSpawnPoint(GameObject h)
    {
        GameObject mapSpawn = map.transform.Find("SpawnArea").gameObject;
        float zValue = mapSpawn.GetComponent<SpawnArea>().size.z / 2;
        float xValue = mapSpawn.GetComponent<SpawnArea>().size.x / 2;
        float spawnZ = Random.Range(mapSpawn.transform.position.z - zValue, mapSpawn.transform.position.z + zValue);
        float spawnX = Random.Range(mapSpawn.transform.position.x - xValue, mapSpawn.transform.position.x + xValue);
        float spawnY = mapSpawn.transform.position.y;
        bool goodSpawn = false;
        int spawnAttemptCount = 0;
        while (!goodSpawn)
        {
            if (spawnAttemptCount >= 3000)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().CantPlaceGoal();
                break;
            }
            spawnZ = Random.Range(mapSpawn.transform.position.z - zValue, mapSpawn.transform.position.z + zValue);
            spawnX = Random.Range(mapSpawn.transform.position.x - xValue, mapSpawn.transform.position.x + xValue);

            Collider[] colliders = Physics.OverlapSphere(new Vector3(spawnX, spawnY, spawnZ), h.transform.localScale.x);
            goodSpawn = true;

            // Must be above map
            int layerMask = LayerMask.GetMask("World");
            RaycastHit hit;
            Debug.DrawRay(new Vector3(spawnX, spawnY, spawnZ), Vector3.down * 50f, Color.red, 10f);
            if (Physics.Raycast(new Vector3(spawnX, spawnY, spawnZ), Vector3.down, out hit, 50f, layerMask))
            {
            }
            else
            {
                goodSpawn = false;
                spawnAttemptCount++;
                continue;
            }


            foreach (Collider c in colliders)
            {
                if (c.tag == "Goal" || c.tag == "Player" || c.tag == "Body" || c.tag == "DeadStar" || c.tag == "Hazzard" || c.tag == "Teleport")
                {
                    goodSpawn = false;
                    break;
                }
            }
            spawnAttemptCount++;
        }

        Vector3 spawnPosition = new Vector3(spawnX, spawnY + h.GetComponent<Renderer>().bounds.size.y/2, spawnZ);
        return spawnPosition;
    }

}
