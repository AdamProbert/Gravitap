using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageHandler : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle explosions;
    public Toggle tutorial;

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

    public void SetHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();

        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore");
    }   

    private void SetPlayerPrefs()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        explosions.isOn = PlayerPrefs.GetInt("explosions") == 1? true : false;
        tutorial.isOn = PlayerPrefs.GetInt("tutorial") == 1? true : false;
    }
}
