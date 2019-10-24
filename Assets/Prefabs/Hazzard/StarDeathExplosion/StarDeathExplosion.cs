using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDeathExplosion : BaseHazzard
{
    private bool shouldBoom = true;
    public override int spawnFrequency { get { return 5; } }
    public override int minScore { get { return 10;} }

    private void Start()
    {
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
            transform.GetChild(0).gameObject.SetActive(false); // Turn off rings
            shouldBoom = false;
            base.HazzardDeath();
        }
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(Parameters.starDestructionLifeTime);
        if (shouldBoom)
        {
            DestoryNeighborStars();
            transform.GetChild(0).gameObject.SetActive(false); // Turn off rings
            base.HazzardDeath();
        }
    }

    private void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, Parameters.starDestructionRadius);
    #endif
    }

    private void DestoryNeighborStars()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Parameters.starDestructionRadius);
        foreach (Collider c in colliders)
        {
            if (c.tag == "Body")
            {
                c.gameObject.GetComponent<Attractor>().DestroySelf();
            }
        }
    }
}
