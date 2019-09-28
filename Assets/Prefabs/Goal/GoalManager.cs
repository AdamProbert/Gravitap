using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour
{
    public GameObject goalPrefab;
    private GameObject currentGoal;
    public GameObject map;
    public float border = 10;
    public int points = 0;
    public Text pointText;

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
            points++;
            pointText.text = points.ToString();
            SpawnGoal();
        }
    }

    // Spawns a new goal at random location on map
    void SpawnGoal()
    {
        float zValue = map.transform.localScale.z/2 - border;
        float xValue = map.transform.localScale.x/2 - border;

        float spawnZ = Random.Range(-zValue, zValue);
        float spawnX = Random.Range(-xValue, xValue);

        Vector3 spawnPosition = new Vector3(spawnX, 1, spawnZ);
        currentGoal = Instantiate(goalPrefab, spawnPosition, Quaternion.identity);

    }
}
