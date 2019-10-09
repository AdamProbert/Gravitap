using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGoal : Goal
{
    private void Start()
    {
        base.Setup();
        base.SetColor(Parameters.colorList[Random.Range(0, Parameters.colorList.Count - 1)]);
    }

    void Update()
    {

        base.Rotate(50, 50, 20);

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            base.GoalDeath();
        }
    }
}
