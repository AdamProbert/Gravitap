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
    public bool showingOptionsMenu = false;
    private StorageHandler sh;
    private ScoreManager sm;
    private GoalManager gm;
    private bool endingGame = false;
    private bool firstGoalSpawned;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager start called");
        sh = GetComponent<StorageHandler>();
        sm = GetComponent<ScoreManager>();
        gm = GetComponent<GoalManager>();
        bm = GameObject.Find("BodyManager").GetComponent<BodyManager>();
        playerScript = player.GetComponent<PlayerScript>();
        ShowMenu();
    }

    public void StartGame()
    {
        Debug.Log("Running routine");
        playing = true;
        StartCoroutine(StartRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn first goal
        if (!firstGoalSpawned && playerScript.isPlaying)
        {
            GetComponent<GoalManager>().SpawnGoal();
            firstGoalSpawned = true;
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
            playing = false;
            endingGame = true;
            int points = sm.GetCurrentScore();
            sh.SetHighScore(points);
            SceneManager.LoadScene(0);
        }
        
    }

    // Close the application
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OptionsPressed()
    {
        // Open options
        if (!showingOptionsMenu)
        {
            if (!playing)
            {
                HideMenu();
                ShowPauseMenu();
            }
            // In game
            else
            {
                Time.timeScale = 0;
                ShowPauseMenu();
                playerScript.isPaused = true;
            }
        }
        // Close options
        else
        {
            if (!playing)
            {
                HidePauseMenu();
                ShowMenu();
            }
            // In game
            else
            {
                Time.timeScale = 1;
                HidePauseMenu();
                playerScript.isPaused = false;
            }
        }
    }

    void ShowMenu()
    {
        Debug.Log("Showing Menu");
        menuAnim.Play("MenuIn");
        showingMenu = true;
    }

    void HideMenu()
    {
        menuAnim.Play("MenuOut");
        showingMenu = false;
    }

    void ShowPauseMenu()
    {
        pauseMenuAnim.Play("ShowPauseMenu");
        showingOptionsMenu = true;
    }

    void HidePauseMenu()
    {
        pauseMenuAnim.Play("HidePauseMenu");
        showingOptionsMenu = false;
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
