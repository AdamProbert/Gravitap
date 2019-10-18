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
        gm.GetComponent<GoalManager>().OnGoalCountChange += GoalCountChangeHandler;
        goalCountTrigger = Parameters.mapChangeGoalCount;
    }

    void GoalCountChangeHandler(int goalCount)
    {
        if (goalCount > goalCountTrigger)
        {
            currentMapIndex += 1;
            if (currentMapIndex <= transform.childCount-1)
            {
                TransportPlayer();
            }
        }
    }

    void TransportPlayer()
    {
        goalCountTrigger += Parameters.mapChangeGoalCount;
        CurrentMap = transform.GetChild(currentMapIndex).gameObject;
        TrailRenderer ptr = player.GetComponent<TrailRenderer>();
        ptr.enabled = false;
        player.transform.position = CurrentMap.transform.Find("SpawnArea").position;
        ptr.enabled = true;
    }
}



//USE THIS MOTHERFUCKER: 
//http://www.procore3d.com/forum/topic/1695-scripting-for-teleport-from-collision-target/
// Also need to destoy all previous hazzards
