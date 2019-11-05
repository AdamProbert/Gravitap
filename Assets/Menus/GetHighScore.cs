using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHighScore : MonoBehaviour
{

    public StorageHandler storageHandler;
    public GameServicesHandler gsh;
    private Text text;
    // Start is called before the first frame update

    int highscore;
    void Start()
    {
        text = GetComponent<Text>();
        highscore = gsh.HighScore;
        if(highscore == -1)
        {
            Debug.Log("GSH high score == -1");
            highscore = storageHandler.highscore;
            if(highscore == -1)
            {
                Debug.Log("SH high score == -1");
                text.text = "Highscore\nnot loaded";
            }
            else
            {
                text.text = "Highscore\n" + highscore.ToString();
            }
            
        }
        else
        {
            text.text = "Highscore\n" + highscore.ToString();
        }
        
        
        gsh.OnHighScoreChange += OnHighScoreChange;
    }

    void OnHighScoreChange(int newscore)
    {
        Debug.Log("On high score changed");
        highscore = newscore;
        text.text = "Highscore\n" + gsh.HighScore.ToString();
    }
}
