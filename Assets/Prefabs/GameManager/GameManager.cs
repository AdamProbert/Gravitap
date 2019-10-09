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
    public Animator hudAnim;
    public Text highscore;
    public bool showingMenu = true;

    // Start is called before the first frame update
    void Start()
    {
        bm = GameObject.Find("BodyManager").GetComponent<BodyManager>();
        playerScript = player.GetComponent<PlayerScript>();
        highscore.text = "HIGHSCORE\n" + GetComponent<StorageHandler>().GetHighScore().ToString();
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
        Debug.Log("Ending game, saving score.");
        int points = GetComponent<ScoreManager>().GetCurrentScore();
        GetComponent<StorageHandler>().SetHighScore(points);
        SceneManager.LoadScene(0);
    }

    void ShowMenu()
    {
        menuAnim.SetBool("isHidden", false);
        showingMenu = true;
    }

    void HideMenu()
    {
        menuAnim.SetBool("isHidden", true);
        showingMenu = false;
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
