using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameServicesHandler : MonoBehaviour
{
    private int gsHighScore = -1;

    public int HighScore
    {
        get { return gsHighScore; }
        set
        {
            if (gsHighScore == value) return;
            gsHighScore = value;
            OnHighScoreChange?.Invoke(gsHighScore);
        }
    }
    public delegate void OnHighScoreChangeDeligate(int gsHighScore);
    public event OnHighScoreChangeDeligate OnHighScoreChange;

    // Checks if EM has been initialized and initialize it if not.
    // This must be done once before other EM APIs can be used.
    void Awake()
    {
        if (!RuntimeManager.IsInitialized())
        {
            RuntimeManager.Init();
            Debug.Log("Initialising runtime manger");
        }
    }

    void Start()
    {
        // Wait for services to initialise, then load users score
        GameServices.LoadLocalUserScore(EM_GameServicesConstants.Leaderboard_HighScore, OnLocalUserScoreLoaded);
    }

    public void SetHighScore(int score)
    {
        SubmitHighScore(score);
    }

    public int GetHighScore()
    {
        return gsHighScore;
    }

    public void ShowLeaderBoardUI()
    {
        // Check for initialization before showing leaderboard UI
        if (GameServices.IsInitialized())
        {
            GameServices.ShowLeaderboardUI();
        }
        else
        {
            Debug.Log("Game services not initialised");
        }
    }

    private void SubmitHighScore(int score)
    {
        // Report a score of 100
        // EM_GameServicesConstants.Sample_Leaderboard is the generated name constant
        // of a leaderboard named "Sample Leaderboard"
        if (GameServices.IsInitialized())
            GameServices.ReportScore(score, EM_GameServicesConstants.Leaderboard_HighScore);
    }

 
    // Score loaded callback
    void OnLocalUserScoreLoaded(string leaderboardName, IScore score)
    {
        if (score != null)
        {
            gsHighScore = (int)score.value;
            Debug.Log("Your score is: " + score.value);
        }
        else
        {
            Debug.Log("You don't have any score reported to leaderboard " + leaderboardName);
        }
    }
}
