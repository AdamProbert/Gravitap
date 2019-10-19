using EasyMobile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameServicesHandler : MonoBehaviour
{
    private int HighScore = -1;

    // Checks if EM has been initialized and initialize it if not.
    // This must be done once before other EM APIs can be used.
    void Awake()
    {
        if (!RuntimeManager.IsInitialized())
            RuntimeManager.Init();

    }

    private void Start()
    {
        if (GameServices.IsInitialized())
        {
            // Load the local user's score from the specified leaderboard
            // EM_GameServicesConstants.Sample_Leaderboard is the generated name constant
            // of a leaderboard named "Sample Leaderboard"
            GameServices.LoadLocalUserScore(EM_GameServicesConstants.Leaderboard_TopScore, OnLocalUserScoreLoaded);
            Debug.Log("Game services initialised");
        }
        else
        {
            Debug.Log("Game Services could not initialise");
        }
    }

    public void SetHighScore(int score)
    {
        SubmitHighScore(score);
    }

    public int GetHighScore()
    {
        return HighScore;
    }

    public void ShowLeaderBoardUI()
    {

        // Check for initialization before showing leaderboard UI
        if (GameServices.IsInitialized())
        {
            GameServices.ShowLeaderboardUI();
        }
    }

    private void SubmitHighScore(int score)
    {
        // Report a score of 100
        // EM_GameServicesConstants.Sample_Leaderboard is the generated name constant
        // of a leaderboard named "Sample Leaderboard"
        if (GameServices.IsInitialized())
            GameServices.ReportScore(score, EM_GameServicesConstants.Leaderboard_TopScore);
    }

 
    // Score loaded callback
    void OnLocalUserScoreLoaded(string leaderboardName, IScore score)
    {
        if (score != null)
        {
            HighScore = (int)score.value;
            Debug.Log("Your score is: " + score.value);
        }
        else
        {
            Debug.Log("You don't have any score reported to leaderboard " + leaderboardName);
        }
    }
}
