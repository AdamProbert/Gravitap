using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGoal : Goal
{
    private string playerTag = "Player";
    private void Start()
    {
        base.Setup();
        base.SetColor(Parameters.colorList[Random.Range(0, Parameters.colorList.Count - 1)]);
    }

    void Update()
    {

        base.Rotate(100, 100, 40);

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == playerTag)
        {
            base.GoalDeath();
        }
    }
}
