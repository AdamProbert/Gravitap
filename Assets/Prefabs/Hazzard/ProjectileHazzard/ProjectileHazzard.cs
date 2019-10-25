using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHazzard : BaseHazzard
{
    public override int spawnFrequency { get { return 1; } }
    public override int minScore { get { return 50; } }
    public GameObject projectilePrefab;
    private GameObject player;

    private float projectileSpeed;
    private Vector3 rotation = new Vector3(90, 270, 0);

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        projectileSpeed = Parameters.projectileSpeed;

        // May need to handle pausing
        InvokeRepeating("LaunchProjectile", 3.0f, 1.0f); // Shoot in x, seconds then every x seconds

    }

    void Update()
    {
        transform.LookAt(player.transform.position);
        transform.Rotate(rotation);
    }

    void LaunchProjectile()
    {
        for(int i = 0; i <= 1; i++){
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Destroy(projectile, 10f); // Destroy self in 10 seconds

            projectile.transform.LookAt(player.transform.position);

            Vector3 heading = (player.transform.position - transform.position);        
            projectile.GetComponent<Rigidbody>().AddForce(heading.normalized*projectileSpeed);
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
