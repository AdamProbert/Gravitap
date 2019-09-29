﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BodyManager : MonoBehaviour
{

    public GameObject star;
    private Queue<GameObject> stars = new Queue<GameObject>();
    private AudioSource source;
    private float[] soundPitches = {0.8f, 0.9f, 1f, 1.1f, 1.2f};
    private int pitchIndex = 0;
    public TextMeshProUGUI killText;
    private int killCount = 0;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        CleanStarQueue();

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.gameObject.tag == "World")
                {
                    Vector3 spawnBody = new Vector3(hit.point.x, 0, hit.point.z);
                    GameObject newStar = Instantiate(star, spawnBody, Quaternion.identity);
                    newStar.transform.parent = transform;
                    playSound();
                    this.stars.Enqueue(newStar);
                }
                else if (hit.collider.gameObject.tag == "BodyClickTrigger")
                {
                    if (hit.collider.gameObject.GetComponentInParent<Attractor>().alive == true)
                        playSound();
                        Destroy(hit.collider.transform.parent.gameObject);
                }
            }
            
        }
    }

    private void CleanStarQueue()
    {
        GameObject oldStar = null;
        // Cycle through queue, remove nulls or dead. Enqueue valid objects
        if (this.stars.Count > Parameters.maxStars)
        {
            int count = this.stars.Count;
            for (int i = 0; i < count; i++)
            {
                oldStar = this.stars.Dequeue();
                if (oldStar != null && oldStar.GetComponent<Attractor>().alive == true)
                {
                    stars.Enqueue(oldStar);
                }
            }          
            
        }
        if (this.stars.Count > Parameters.maxStars)
        {
            oldStar = this.stars.Dequeue();
            Destroy(oldStar);
        }
    }

    public int getKillCount()
    {
        return killCount;
    }

    public void registerKill()
    {
        killCount += 1;
        updatedKillText();
    }

    public void resetKills()
    {
        killCount = 0;
        updatedKillText();
    }

    public void updatedKillText()
    {
        killText.text = new String('X', killCount);
        killText.fontSize = 100;
        StartCoroutine(ResetFontSize());
    }

    IEnumerator ResetFontSize()
    {
        yield return new WaitForSeconds(.5f);
        killText.fontSize = 70;
    }

    // Play AudioSource sound at increasing pitches
    private void playSound()
    {
        source.pitch = soundPitches[pitchIndex];
        source.Play();
        pitchIndex++;
        if(pitchIndex >= soundPitches.Length)
        {
            pitchIndex = 0;
        }
    }
}