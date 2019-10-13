using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI multiplierText;
    private Animator anim;
    private int currentMultiplier;
    public int m_currentPoints;
    private List<GameObject> triggeredStars = new List<GameObject>();
    public GameObject goalScoreText;

    // Use property for points. Allows callbacks on change with the delegate function below.
    // Hella cool
    public int CurrentPoints
    {
        get { return m_currentPoints; }
        set
        {
            if (m_currentPoints == value) return;
            m_currentPoints = value;
            if (OnScoreChange != null)
            {
                Debug.Log("Ne score set");
                OnScoreChange(m_currentPoints);
            }              
        }
    }

    public delegate void OnScoreChangeDelegate(int newScore);
    public event OnScoreChangeDelegate OnScoreChange;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentPoints = Parameters.startPoints;
        currentMultiplier = Parameters.startMultiplier;
        anim = pointText.GetComponent<Animator>();
    }

    public int GetCurrentScore()
    {
        return CurrentPoints;
    }

    public void DoubleMutliplier()
    {
        Debug.Log("Doubling multiplier");
        currentMultiplier *= 2;
        UpdateMultiplierText();
    }

    public void HitGoal(GameObject goal)
    {
        Debug.Log("Player hit goal");
        int stars = triggeredStars.Count + 1;
        int points = Parameters.goalValue * currentMultiplier;
        CurrentPoints += points;
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
        if(currentMultiplier > 1)
        {
            currentMultiplier -= 1;
            UpdateMultiplierText();
        }
    }

    public void HitStar()
    {
        triggeredStars.Clear();
        currentMultiplier = Parameters.startMultiplier;
        UpdateMultiplierText();
    }

    private void ShowFloatingText(Transform transform, string text)
    {
        Debug.Log("Showing floating text: " +text);
        GameObject scoreText = Instantiate(goalScoreText, transform.position, transform.rotation, transform);
        scoreText.GetComponent<TextMesh>().text = text;
        scoreText.transform.rotation = Quaternion.Euler(90, -40, 0);
    }

    private void UpdatePointText()
    {
        anim.SetTrigger("updateScore");
        pointText.text = "" + CurrentPoints;
    }

    private void UpdateMultiplierText()
    {
        multiplierText.text = "x" + currentMultiplier;
    }
}


