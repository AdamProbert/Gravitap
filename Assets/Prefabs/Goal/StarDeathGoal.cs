using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDeathGoal : Goal
{
    BodyManager bm;
    private bool shouldBoom = true;

    private void Start()
    {
        base.Setup();
        base.SetColor(Color.black);
        bm = transform.root.gameObject.GetComponentInChildren<BodyManager>(); // Gets the root game object. E.g. the GameManager
        StartCoroutine(SelfDestruct());

    }

    void Update()
    {

        base.Rotate(0, 180, 0);

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            shouldBoom = false;
            base.GoalDeath(false);
        }
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(Parameters.deathGoalLifeTime);
        if (shouldBoom)
        {
            DestoryNeighborStars();
            transform.GetChild(0).gameObject.SetActive(false);
            base.GoalDeath(false);
        }
    }

    private void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, Parameters.starGoalDestructionRadius);
    #endif
    }

    private void DestoryNeighborStars()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Parameters.starGoalDestructionRadius);
        foreach (Collider c in colliders)
        {
            if (c.tag == "Body")
            {
                c.gameObject.GetComponent<Attractor>().DestroySelf();
            }
        }
    }
}
