using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileInputController : MonoBehaviour
{
    public PlayerScript player;
    public Camera camera;
    public LayerMask mask;
    private BodyManager bm;
    private ScoreManager sm;
    public AudioClip buttonPress;
    private AudioSource audio;

    private void Start()
    {
        bm = GetComponentInChildren<BodyManager>();
        sm = GetComponent<ScoreManager>();
        audio = GetComponent<AudioSource>();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackPressed()
    {
        if (GetComponent<GameManager>().showingMenu)
        {
            QuitGame();
        }
        else
        {
            GetComponent<GameManager>().EndGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackPressed();
        }

        if(player)
        {
            if (Input.GetMouseButtonDown(0) & player.isPlaying)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
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
        audio.PlayOneShot(buttonPress);
    }
}