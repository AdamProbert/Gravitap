﻿using System.Collections;
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
    public MapManager mm; // From editor
    public PlayerScript player;
    private ScoreManager sm;
    private int m_goalCount = 0;


    public int CurrentGoalCount
    {
        get { return m_goalCount; }
        set
        {
            if (m_goalCount == value) return;
            m_goalCount = value;
            OnGoalCountChange?.Invoke(m_goalCount);
        }
    }
    public delegate void OnGoalCountChangeDeligate(int newGoalCount);
    public event OnGoalCountChangeDeligate OnGoalCountChange;


    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.Find("BodyManager").GetComponent<BodyManager>();
        border = Parameters.border;
        sm = GetComponent<ScoreManager>();
        mm.OnMapChange += OnMapChangeHandler;
    }


    public void GoalDeath(GameObject goal)
    {
        if (player.isPlaying)
        {
            Debug.Log("Registered goal death with manager and player is playing");
            sm.HitGoal(goal);
            
            // Only spawn new goal if normal goal destroyed
            if(goal.GetComponent<NormalGoal>() != null)
            {
                CurrentGoalCount += 1;
                SpawnGoal();
            }
        }
    }

    // Spawns a new goal at random location on map
    public void SpawnGoal()
    {
        currentGoal = Instantiate(goalPrefab, GetSpawnPoint(), Quaternion.identity);
        currentGoal.transform.parent = transform;
    }

    public GameObject GetCurrentGoal()
    {
        return currentGoal;
    }

    void OnMapChangeHandler(GameObject newMap)
    {
        Debug.Log("GoalManager: Map changed, spawning new goal");
        map = newMap;
        Destroy(currentGoal);
        SpawnGoal();
    }

    public Vector3 GetSpawnPoint()
    {
        GameObject mapSpawn = map.transform.GetChild(1).gameObject;
        float zValue = mapSpawn.GetComponent<SpawnArea>().size.z/2;
        float xValue = mapSpawn.GetComponent<SpawnArea>().size.x/2;
        float spawnZ = Random.Range(mapSpawn.transform.position.z - zValue, mapSpawn.transform.position.z + zValue);
        float spawnX = Random.Range(mapSpawn.transform.position.x - xValue, mapSpawn.transform.position.x + xValue);
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

            Collider[] colliders = Physics.OverlapSphere(new Vector3(spawnX, mapSpawn.transform.position.y, spawnZ), goalPrefab.transform.localScale.x);
            goodSpawn = true;

            // Must be above map
            int layerMask = LayerMask.GetMask("World");
            Debug.Log("Checking if spawn above ground");
            RaycastHit hit;
            Debug.DrawRay(new Vector3(spawnX, mapSpawn.transform.position.y, spawnZ), Vector3.down * 50f, Color.red, 10f);
            if(Physics.Raycast(new Vector3(spawnX, mapSpawn.transform.position.y, spawnZ), Vector3.down, out hit, 50f, layerMask))
            {
                Debug.Log("Hit:" + hit.transform.gameObject.name);
            }
            else
            {
                Debug.Log("Didn't hit anything");
                goodSpawn = false;
                spawnAttemptCount++;
                continue;
            }


            Debug.Log("Checking collisions");
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

        Vector3 spawnPosition = new Vector3(spawnX, mapSpawn.transform.position.y, spawnZ);
        Debug.Log("Took " + spawnAttemptCount + " attempts to spawn");
        return spawnPosition;
    }

}
