using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public GameObject goalPrefab;
    private GameObject currentGoal;

    // Start is called before the first frame update
    void Start()
    {
        SpawnGoal();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGoal == null)
        {
            SpawnGoal();
        }
    }

    // Spawns a new goal at random location on screen
    void SpawnGoal()
    {
        float spawnY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        currentGoal = Instantiate(goalPrefab, spawnPosition, Quaternion.identity);

    }
}
