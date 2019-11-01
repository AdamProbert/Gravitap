using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    Vector3 targetPostition = Vector3.zero;
    public GoalManager goalManager;

    void Update()
    {
        FindTarget();
        transform.LookAt(targetPostition);
        transform.Rotate(0, -90, 0); // rotate back 90 cause weird shiz, trust me. Leave it as is.

    }


    // Finds the closest goal to the player
    void FindTarget()
    {
        
        List<GameObject> goals = goalManager.GetAllGoals();        
        GameObject closestGoal = null;
        foreach(GameObject g in goals)
        {                
            if (g.GetComponent<NormalGoal>() != null && g.GetComponent<Goal>().alive)
                if(!closestGoal)
                {
                    closestGoal = g;
                }
                else
                {
                    if(Vector3.Distance(transform.position, g.transform.position) < Vector3.Distance(transform.position, closestGoal.transform.position))
                    {
                        closestGoal = g;                        
                    }
                }
        }
        if(closestGoal)
        {
            targetPostition = closestGoal.transform.position;
            GetComponentInChildren<Renderer>().enabled = true;
        }
        else
        {
            GetComponentInChildren<Renderer>().enabled = false;
        }
        
    }
}
