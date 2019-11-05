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

    public int highscore = -1;

    void Start()
    {
        gsh.OnHighScoreChange += GSHighScoreChangeHandler;
    }

    public void Awake()
    {

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
        if (!PlayerPrefs.HasKey("playcount"))
        {
            PlayerPrefs.SetInt("playcount", 0);
        }

        PlayerPrefs.Save();

        SetPlayerPrefs();
    }

    void GSHighScoreChangeHandler(int newScore)
    {
        Debug.Log("StorageHandler: GSHHighScoreChangeHanlder called with: " + newScore);
        PlayerPrefs.SetInt("highscore", newScore);
        PlayerPrefs.Save();
    }

    private void SetPlayerPrefs()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        explosions.isOn = PlayerPrefs.GetInt("explosions") == 1? true : false;
        tutorial.isOn = PlayerPrefs.GetInt("tutorial") == 1? true : false;
    }

    public int GetPlayCount()
    {
        return PlayerPrefs.GetInt("playcount");
    }

    public void IncrementPlayCount()
    {
        int count = GetPlayCount();
        PlayerPrefs.SetInt("playcount", count+=1);
    }

    // If player will get an ad next time they load the app reduce the play count by 1, so they have to play at least
    // Once next time before getting that ad.
    void OnApplicationQuit()
    {
        int count = GetPlayCount();
        if(count % Parameters.livesForAdd == 0 && count != 0)
        {
            PlayerPrefs.SetInt("playcount", count-=1);
        }
    }
}
