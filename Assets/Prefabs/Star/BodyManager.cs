using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{

    public GameObject star;
    private Queue<GameObject> stars = new Queue<GameObject>();
    private ScoreManager sm;
    private GameManager gm;

    // Sound
    private AudioSource source;
    private float startSoundPitch = 0.8f;
    private float soundPitch;

    // State
    public bool affectingPlayer = false; // Any stars currently affecting player?

    private void Start()
    {
        source = GetComponent<AudioSource>();
        sm = GetComponentInParent<ScoreManager>();
        soundPitch = startSoundPitch;
        gm = GetComponentInParent<GameManager>();
    }

    private void LateUpdate()
    {
        if (gm.GetComponent<GameManager>().playing)
        {
            CleanStarQueue();
            LastStarIdentifier();
        }
    }

    public void SpawnStar(RaycastHit hit)
    {
        Vector3 spawnBody = new Vector3(hit.point.x, hit.point.y+(star.GetComponent<Renderer>().bounds.size.y/2), hit.point.z);
        GameObject newStar = Instantiate(star, spawnBody, Quaternion.identity);
        newStar.transform.parent = transform;
        PlaySound();
        this.stars.Enqueue(newStar);
    }

    public void RemoveStar(RaycastHit hit)
    {
        PlaySound(true);
        hit.collider.transform.parent.gameObject.GetComponent<Attractor>().RemoveSelf();
    }

    // Halve the alpha of the final star in the queue
    private void LastStarIdentifier()
    {
        if(stars.Count > 0)
        {
            GameObject lastStar = stars.Peek();
            if (stars.Count == 5)
                lastStar.GetComponent<Renderer>().material.color = Parameters.orange;
            else
                lastStar.GetComponent<Renderer>().material.color = Parameters.red;
        }
    }

    private void CleanStarQueue()
    {
        // Flag for any star this frame affecting player
        bool frameAffectingPlayer = false;
        GameObject oldStar = null;
        // Cycle through queue, remove nulls or dead. Enqueue valid objects
        int count = this.stars.Count;
        for (int i = 0; i < count; i++)
        {
            oldStar = this.stars.Dequeue();
            if (oldStar != null)
            {
                if (oldStar.GetComponent<Attractor>().playerTouch)
                {
                    frameAffectingPlayer = true;
                }

                stars.Enqueue(oldStar);
            }
        }          
        // After clean, if still to many. Remove last one
        if (this.stars.Count > Parameters.maxStars)
        {
            oldStar = this.stars.Dequeue();
            Destroy(oldStar);
        }

        // If no active stars affecting player. Inform score manager to reset combo
        if (!frameAffectingPlayer)
        {
            sm.PlayerLeftGravity();
            affectingPlayer = false;
            ResetPitch();
        }
        else
        {                                                                                                               
            affectingPlayer = true;
        }

    }

    public void KillAllBodies()
    {
        foreach(GameObject star in stars)
        {
            star.GetComponent<Attractor>().DestroySelf();
        }
    }

    // Called from a child Star when star dies
    public void StarDeath()
    {
        ResetPitch();
        sm.HitStar();
    }

    public void ResetPitch()
    {
        soundPitch = startSoundPitch;

    }

    // Play AudioSource sound at increasing pitches
    private void PlaySound(bool remove = false)
    {
        if (remove)
        {
            source.pitch = startSoundPitch;
            source.Play();
        }
        else
        {
            source.pitch = soundPitch;
            source.Play();
            if (soundPitch > 1.1)
            {
                soundPitch += 0.01f;
            }
            else
            {
                soundPitch += 0.05f;
            }
        }
    }
}
