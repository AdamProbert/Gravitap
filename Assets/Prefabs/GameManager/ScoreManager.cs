using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI multiplierText;
    private int currentPoints;
    private int currentMultiplier;
    private List<GameObject> triggeredStars = new List<GameObject>();
    public GameObject goalScoreText;

    // Logic overview.
    // Player increases multiplier by 1 for every new star they come into contact with
    // Score = triggeredStars.count *1
    // If player leaves gravity of all stars multiplier is reset
    // If Player hit's star, multiplier is reset.


    // Start is called before the first frame update
    void Start()
    {
        currentPoints = Parameters.startPoints;
        currentMultiplier = Parameters.startMultiplier;
    }

    public int GetCurrentScore()
    {
        return currentPoints;
    }

    public void DoubleMutliplier()
    {
        Debug.Log("Doubling multiplier");
        currentMultiplier *= 2;
        UpdateMultiplierText();
    }

    public void HitGoal(GameObject goal)
    {
        int stars = triggeredStars.Count + 1;
        int points = Parameters.goalValue * stars;
        currentPoints += points;
        UpdatePointText();

        if (goal.GetComponent<MultiplierGoal>() != null)
        {
            ShowFloatingText(goal.transform, "x2");
        }
        else
        {

            ShowFloatingText(goal.transform, points.ToString());
        }
    }

    public void PlayerLeftGravity()
    {
        triggeredStars.Clear();
        currentMultiplier = Parameters.startMultiplier;
        UpdateMultiplierText();
    }

    public void TriggeredStar(GameObject star)
    {
        if (!triggeredStars.Contains(star))
        {
            triggeredStars.Add(star);
            currentMultiplier += 1;
            UpdateMultiplierText();
        }            
    }

    public void RemoveStar(GameObject star)
    {
        //if (triggeredStars.Contains(star))
        //{
        //    triggeredStars.Clear();
        //}
    }

    public void HitStar()
    {
        triggeredStars.Clear();
        currentMultiplier = Parameters.startMultiplier;
        UpdateMultiplierText();
    }

    private void ShowFloatingText(Transform transform, string text)
    {
        GameObject scoreText = Instantiate(goalScoreText, transform.position, transform.rotation, transform);
        scoreText.GetComponent<TextMesh>().text = text;
        scoreText.transform.rotation = Quaternion.Euler(90, -40, 0);
    }

    private void UpdatePointText()
    {
        pointText.text = "" + currentPoints;
        pointText.fontSize = Parameters.largeScoreSize;
        StartCoroutine(ResetFontSize(pointText, Parameters.normalScoreSize));
    }

    private void UpdateMultiplierText()
    {
        multiplierText.text = "x" + currentMultiplier;
        //multiplierText.fontSize = Parameters.largeMultiplierSize;
        //StartCoroutine(ResetFontSize(multiplierText, Parameters.normalMultiplierSize));
    }

    IEnumerator ResetFontSize(TextMeshProUGUI text, int fontSize)
    {
        yield return new WaitForSeconds(Parameters.textResetTime);
        text.fontSize = fontSize;
    }
}


