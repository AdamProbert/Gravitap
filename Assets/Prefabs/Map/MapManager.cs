using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private GameObject m_currentMap;
    public GameObject gm; // From editor
    private int goalCountTrigger;
    private int currentMapIndex = 0;
    public GameObject player; // From editor
    public GameObject leveltp;
    GameObject teleport;
    public int mapCount = 4; // MUST KEEP THIS UP TO DATE WITH NUMBER OF MAPS


    public GameObject CurrentMap
    {
        get { return m_currentMap; }
        set
        {
            if (m_currentMap == value) return;
            m_currentMap = value;
            OnMapChange?.Invoke(m_currentMap);
        }
    }
    public delegate void OnMapChangeDelegate(GameObject map);
    public event OnMapChangeDelegate OnMapChange;

    // Start is called before the first frame update
    void Start()
    {
        CurrentMap = transform.GetChild(currentMapIndex).gameObject;
        gm.GetComponent<GoalManager>().OnGoalCountChange += GoalCountChangeHandler;
        goalCountTrigger = Parameters.mapChangeGoalCount;
    }

    void GoalCountChangeHandler(int goalCount)
    {
        if (goalCount >= goalCountTrigger && !teleport)
        {
            currentMapIndex += 1;
            if (currentMapIndex <= mapCount-1)
            {
                SpawnTeleport();
            }
        }
    }

    // Spawn teleport at centre point and surface of map
    void SpawnTeleport()
    {
        Vector3 location = CurrentMap.GetComponentInChildren<SpawnArea>().transform.position;
        RaycastHit hit;
        if (Physics.Raycast(location, Vector3.down, out hit, 10f, LayerMask.GetMask("World")))
        {
            location.y -= hit.distance;
            teleport = Instantiate(leveltp, location, Quaternion.identity, transform);
        }
    }

    public void TransportPlayer()
    {
        player.GetComponent<PlayerScript>().isTeleporting = true;
        goalCountTrigger += Parameters.mapChangeGoalCount;
        CurrentMap = transform.GetChild(currentMapIndex).gameObject;

        // Move teleport
        teleport.GetComponent<Collider>().enabled = false;
        teleport.transform.position = CurrentMap.GetComponentInChildren<SpawnArea>().transform.position;
        Destroy(teleport, 3f);
        
        // Move player
        Vector3 mapSpawn = CurrentMap.transform.Find("SpawnArea").gameObject.transform.position;
        player.GetComponent<PlayerScript>().Teleport(mapSpawn);

    }
}