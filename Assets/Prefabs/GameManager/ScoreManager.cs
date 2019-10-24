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
    private int m_currentMultiplier;
    private int m_currentPoints;
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
            OnScoreChange?.Invoke(m_currentPoints);
        }
    }
    public delegate void OnScoreChangeDelegate(int newScore);
    public event OnScoreChangeDelegate OnScoreChange;

    public int CurrentMultiplier
    {
        get { return m_currentMultiplier; }
        set
        {
            if (m_currentMultiplier == value) return;
            m_currentMultiplier = value;
            OnMultiplierChange?.Invoke(m_currentMultiplier);
        }
    }
    public delegate void OnMultiplierChangeDelegate(int newScore);
    public event OnMultiplierChangeDelegate OnMultiplierChange;

    // Start is called before the first frame update
    void Start()
    {
        CurrentPoints = Parameters.startPoints;
        CurrentMultiplier = Parameters.startMultiplier;
        anim = pointText.GetComponent<Animator>();
    }

    public int GetCurrentScore()
    {
        return CurrentPoints;
    }

    public void DoubleMutliplier()
    {
        CurrentMultiplier *= 2;
        UpdateMultiplierText();
    }

    public void HitGoal(GameObject goal)
    {
        int stars = triggeredStars.Count + 1;
        int points = Parameters.goalValue * CurrentMultiplier;
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
        CurrentMultiplier = Parameters.startMultiplier;
        UpdateMultiplierText();
    }

    public void TriggeredStar(GameObject star)
    {
        if (!triggeredStars.Contains(star))
        {
            triggeredStars.Add(star);
            CurrentMultiplier += 1;
            UpdateMultiplierText();
        }            
    }

    public void RemoveStar(GameObject star)
    {
        if(CurrentMultiplier > 1)
        {
            CurrentMultiplier -= 1;
            UpdateMultiplierText();
        }
    }

    public void HitStar()
    {
        triggeredStars.Clear();
        CurrentMultiplier = Parameters.startMultiplier;
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
        anim.SetTrigger("updateScore");
        pointText.text = "" + CurrentPoints;
    }

    private void UpdateMultiplierText()
    {
        multiplierText.text = "x" + CurrentMultiplier;
    }
}


