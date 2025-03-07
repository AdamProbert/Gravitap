﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MobileInputController : MonoBehaviour
{
    public PlayerScript player;
    public Camera mainCamera;
    public LayerMask mask;
    private BodyManager bm;
    private ScoreManager sm;
    public AudioClip buttonPress;
    private AudioSource audiosource;
    private GameManager gm;
    public AudioSource starsSound;

    public bool isPaused = false;

    private void Start()
    {
        bm = GetComponentInChildren<BodyManager>();
        sm = GetComponent<ScoreManager>();
        gm = GetComponent<GameManager>();
        audiosource = GetComponent<AudioSource>();
    }

    public void BackPressed()
    {
        if (gm.showingMenu)
        {
            gm.QuitGame();
        }
        else
        {
            gm.EndGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackPressed();
        }

        if(player && !isPaused)
        {
            if (Input.GetMouseButtonDown(0) && player.isPlaying && !IsPointerOverUIObject())
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, mask))
                {
                    Debug.DrawRay(ray.origin, ray.direction * 500f, Color.black, 10f);
                    if (hit.collider.gameObject.tag == "World")
                    {
                        bm.SpawnStar(hit);
                    }

                    else if (hit.collider.gameObject.tag == "BodyClickTrigger")
                    {
                        sm.RemoveStar(hit.collider.transform.parent.gameObject);
                        bm.RemoveStar(hit);
                    }
                }
            }
        }
    }

    //When Touching UI
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void PlayButtonSound()
    {
        audiosource.PlayOneShot(buttonPress);
    }

    public void VolumeChange(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        AudioListener.volume = volume;
    }

    public void ToggleExplosions(bool toggle)
    {
        PlayerPrefs.SetInt("explosions", toggle ? 1 : 0);
        GetComponent<DebrisPooler>().playerSaysYehaw = toggle;
    }
    public void ToggleHints(bool toggle)
    {
        PlayerPrefs.SetInt("tutorial", toggle ? 1 : 0);
        if(toggle)
        {
            GetComponent<HintHandler>().enableTutorial();
        }
        else
        {
            GetComponent<HintHandler>().disableTutorial();   
        }
        
    }
}