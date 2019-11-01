using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHighScore : MonoBehaviour
{

    //private GameServicesHandler gs;
    private StorageHandler sh;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        sh = transform.root.GetComponent<StorageHandler>();
        Debug.Log("Getting high score from storage");
        text.text = "Highscore\n" + sh.HighScore.ToString();
        sh.SHOnHighScoreChange += OnHighScoreChange;
    }

    void OnHighScoreChange(int newscore)
    {
        text.text = "Highscore\n" + sh.HighScore.ToString();
    }
}
