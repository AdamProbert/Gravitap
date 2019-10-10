using System.Collections;
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

        if(player && !gm.showingPauseMenu)
        {
            if (Input.GetMouseButtonDown(0) && player.isPlaying && !EventSystem.current.IsPointerOverGameObject()))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, mask))
                {
                    Debug.DrawRay(ray.origin, ray.direction * 500f, Color.black, 10f);
                    Debug.Log("User clicked on " + hit.collider.gameObject.tag);
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

    public void PlayButtonSound()
    {
        audiosource.PlayOneShot(buttonPress);
    }
}