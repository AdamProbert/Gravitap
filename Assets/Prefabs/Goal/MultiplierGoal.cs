using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierGoal : Goal 
{

    ScoreManager sm;

    void Start()
    {
        base.Setup();
        base.SetColor(Parameters.green);
        sm = transform.root.gameObject.GetComponent<ScoreManager>(); // Gets the root game object. E.g. the GameManager
    }

    // Update is called once per frame
    void Update()
    {
        base.Rotate(50, 50, 20);        

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sm.DoubleMutliplier();
            base.GoalDeath(false);
        }
    }
}
