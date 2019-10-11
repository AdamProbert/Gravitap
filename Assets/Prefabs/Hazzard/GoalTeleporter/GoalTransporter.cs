using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTransporter : BaseHazzard
{
    public override int spawnFrequency { get { return 1; } }
    public override int minScore { get { return 10; } }

    private GoalManager gm;
    public GameObject ps;
    private float nextTransportTime;
    private float psDuration;

    private void Start()
    {
        psDuration = ps.GetComponent<ParticleSystem>().main.duration;
        nextTransportTime = Time.time + Parameters.transportGoalDelay;
        gm = transform.root.GetComponent<GoalManager>();
    }

    void Update()
    {
        base.Rotate(180, 180, 180);
        if(nextTransportTime <= Time.time)
        {
            StartCoroutine(TransportGoal());
            nextTransportTime = Time.time + Parameters.transportGoalDelay;
        }
    }

    private IEnumerator TransportGoal()
    {
        // Play Particle system on transporter and remote goal location
        // Then move the goal to new "allowed" position
        if (base.alive)
        {
            GameObject cg = gm.GetCurrentGoal();
            GameObject ps1 = Instantiate(ps, transform.position, Quaternion.identity);
            GameObject ps2 = Instantiate(ps, cg.transform.position, Quaternion.identity);
            Destroy(ps1, psDuration);
            Destroy(ps2, psDuration);
            yield return new WaitForSeconds(psDuration);
            cg.transform.position = gm.GetSpawnPoint();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            base.HazzardDeath();
        }
    }
}
