using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintHandler : MonoBehaviour
{
    // Hints to show
    // - Click to place star    : After startup routine
    // - Click to remove star   : When leave gravity
    // - Chain gravities together for more multiplier! : After second gravity encounter
    // - Avoid or Target the Hazzards   : On first hazzard

    GameManager gameManager;
    public BodyManager bodyManager;
    HazzardHandler hazzardHandler;

    public PlayerScript playerScript;

    public Animator hintAnim;

    public TextMeshProUGUI hintText;



    public string[] messages;
    public int messageIndex = 0;
    private bool hintShowing = false;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        hazzardHandler = GetComponent<HazzardHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!hintShowing && messageIndex <= messages.Length)
        {
            switch (messageIndex)
            {
                case 0:
                    if(playerScript.isPlaying){
                        StartCoroutine(showHint());
                        messageIndex += 1;
                    }
                    break;
                case 1:
                    if(bodyManager.GetStarCount() >= 3)
                    {
                        StartCoroutine(showHint());
                        messageIndex += 1;
                    }
                    break;
                case 2:
                    if(bodyManager.GetStarCount() >= 4)
                    {
                        StartCoroutine(showHint());
                        messageIndex += 1;
                    }
                    break;
                case 3:
                    if(hazzardHandler.getHazzardCount() >= 1)
                    {
                        StartCoroutine(showHint());
                        messageIndex += 1;
                    }
                    break;
            }
        }
    }

    IEnumerator showHint()
    {
        hintShowing = true;       
        hintText.text = messages[messageIndex];
        gameManager.pause();
        hintAnim.Play("ShowHint");
        yield return waitForInput();
        hideHint();
    }

    void hideHint(){

        hintShowing = false;
        hintAnim.Play("HideHint");
        gameManager.unPause();

    }

    private IEnumerator waitForInput()
    {
        bool done = false;
        while(!done) // essentially a "while true", but with a bool to break out naturally
        {
            if(Input.GetMouseButtonDown(0))
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        } 
    }
}
