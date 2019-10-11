using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHighScore : MonoBehaviour
{

    private StorageHandler sh;

    // Start is called before the first frame update
    void Start()
    {
        sh = transform.root.GetComponent<StorageHandler>();
        GetComponent<Text>().text = "Highscore\n" + sh.GetHighScore().ToString();
    }
}
