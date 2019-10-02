using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalManager : MonoBehaviour
{
    public GameObject goalPrefab;
    private GameObject currentGoal;
    public GameObject map;
    private float border;
    public int points = 0;
    public TextMeshProUGUI pointText;
    private BodyManager bm;
    public PlayerScript player;

    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.Find("BodyManager").GetComponent<BodyManager>();
        border = Parameters.border;
    }

    // Update is called once per frame
    void Update()
    {
        if(!currentGoal & player.isPlaying)
        {
            SpawnGoal();
        }
        else if(!currentGoal & !player.isPlaying)
        {
            // Do nothing
        }

        else if (!currentGoal.GetComponent<Goal>().alive & player.isPlaying)
        {
            bm.resetKills();
            points++;
            pointText.text = points.ToString();
            pointText.fontSize = 300;
            StartCoroutine(ResetFontSize());
            SpawnGoal();
        }
    }

    IEnumerator ResetFontSize()
    {
        yield return new WaitForSeconds(.5f);
        pointText.fontSize = 200;
    }

    // Spawns a new goal at random location on map
    void SpawnGoal()
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
                if(c.tag == "Goal" || c.tag == "Player" || c.tag == "Body")
                {
                    goodSpawn = false;
                    break;
                }
            }
            spawnAttemptCount++;
        }

        Vector3 spawnPosition = new Vector3(spawnX, 1, spawnZ);
        currentGoal = Instantiate(goalPrefab, spawnPosition, Quaternion.identity);
    }
}
