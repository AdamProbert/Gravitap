using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    private PlayerScript playerScript;
    private BodyManager bm;
    public bool playing = false;
    public Animator menuAnim;
    public Animator pauseMenuAnim;
    public Animator hudAnim;
    public Text highscore;
    public bool showingMenu = true;
    public bool showingPauseMenu = false;
    private StorageHandler sh;
    private ScoreManager sm;
    private bool endingGame = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager start called");
        sh = GetComponent<StorageHandler>();
        sm = GetComponent<ScoreManager>();
        bm = GameObject.Find("BodyManager").GetComponent<BodyManager>();
        playerScript = player.GetComponent<PlayerScript>();
        ShowMenu();
    }

    public void StartGame()
    {
        Debug.Log("Running routine");
        StartCoroutine(StartRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing && playerScript.isPlaying)
        {
            GetComponent<GoalManager>().SpawnGoal();
            playing = true;
        }
            
        DetectDeath();
    }
    
    public void CantPlaceGoal()
    {
        Debug.Log("Cant place goal, quiting");
        EndGame();
    }

    void DetectDeath()
    {
        if (playerScript && !playerScript.alive)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (!endingGame)
        {
            endingGame = true;
            StartCoroutine(EndRoutine());
        }
        
    }

    // Close the application
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PausePressed()
    {
        Time.timeScale = 0;
        ShowPauseMenu();
        playerScript.isPaused = true;
    }

    public void ResumePressed()
    {
        Time.timeScale = 1;
        HidePauseMenu();
        playerScript.isPaused = false;

    }

    void ShowMenu()
    {
        Debug.Log("Showing Menu");
        menuAnim.SetBool("isHidden", false);
        showingMenu = true;
    }

    void HideMenu()
    {
        menuAnim.SetBool("isHidden", true);
        showingMenu = false;
    }

    void ShowPauseMenu()
    {
        pauseMenuAnim.Play("ShowPauseMenu");
        showingPauseMenu = true;
    }

    void HidePauseMenu()
    {
        pauseMenuAnim.Play("HidePauseMenu");
        showingPauseMenu = false;
    }

    private IEnumerator EndRoutine()
    {
        Debug.Log("Start end routine");
        int points = sm.GetCurrentScore();
        Debug.Log("Saving score of " + points);
        sh.SetHighScore(points);
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(0);
    }

    private IEnumerator StartRoutine()
    {
        Debug.Log("start start routine");
        HideMenu();
        yield return new WaitForSeconds(.5f);
        hudAnim.SetBool("isHidden", false);
        yield return new WaitForSeconds(1.5f);
        transform.Find("PlayerSpawn").GetComponent<PlayerSpawn>().Spawn();
        //transform.Find("HUD").gameObject.SetActive(true);
        Debug.Log("Finish start routine");

    }
}
