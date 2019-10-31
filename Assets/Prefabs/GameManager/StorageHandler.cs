using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageHandler : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle explosions;
    public Toggle tutorial;

    public GameServicesHandler gsh;

    private int shHighScore = -1;
    public int HighScore
    {
        get { return shHighScore; }
        set
        {
            if (shHighScore == value) return;
            shHighScore = value;
            SHOnHighScoreChange?.Invoke(shHighScore);
        }
    }
    public delegate void OnHighScoreChangeDeligate(int shHighScore);
    public event OnHighScoreChangeDeligate SHOnHighScoreChange;

    void Start()
    {
        gsh.OnHighScoreChange += GSHighScoreChangeHandler;
        HighScore = PlayerPrefs.GetInt("highscore"); // First set highscore to playerprefs score

    }
    public void Awake()
    {
        if (!PlayerPrefs.HasKey("highscore"))
        {
            PlayerPrefs.SetInt("highscore", 0);
        }

        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1);
        }

        if (!PlayerPrefs.HasKey("explosions"))
        {
            PlayerPrefs.SetInt("explosions", 1);
        }

        if (!PlayerPrefs.HasKey("tutorial"))
        {
            PlayerPrefs.SetInt("tutorial", 1);
        }

        PlayerPrefs.Save();

        SetPlayerPrefs();
    }

    void GSHighScoreChangeHandler(int newScore)
    {
        Debug.Log("StorageHandler: GSHHighScoreChangeHanlder called with: " + newScore);
        PlayerPrefs.SetInt("highscore", newScore);
        PlayerPrefs.Save();
        HighScore = newScore;
    }

    public void SetHighScore(int score)
    {
        
        if(score > HighScore){
            HighScore = score;
        }
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
        }
        if(score > gsh.HighScore)
        {
            gsh.SetHighScore(score);
        }
    }

    private void SetPlayerPrefs()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        explosions.isOn = PlayerPrefs.GetInt("explosions") == 1? true : false;
        tutorial.isOn = PlayerPrefs.GetInt("tutorial") == 1? true : false;
    }
}
