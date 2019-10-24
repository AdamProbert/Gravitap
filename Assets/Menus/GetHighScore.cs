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
        //gs = transform.root.GetComponent<GameServicesHandler>();
        sh = transform.root.GetComponent<StorageHandler>();
        //long highscore = gs.GetHighScore();

        //if(highscore != -1)
        //{
        //    Debug.Log("Getting high score from GameServices");
        //    text.text = "Highscore\n" + highscore.ToString();
        //}
        //else
        //{
        Debug.Log("Getting high score from storage");
        text.text = "Highscore\n" + sh.GetHighScore().ToString();
        //}
        
    }
}
