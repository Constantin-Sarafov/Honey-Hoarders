using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [Header("HUD Display")]
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        RefreshUI();
    }

    void OnEnable()
    {
        ScoreManager.OnScoreUpdated += RefreshUI;
        RefreshUI();
    }

    void OnDisable()
    {
        ScoreManager.OnScoreUpdated -= RefreshUI;
    }

    void RefreshUI()
    {
        if (currentScoreText != null)
            currentScoreText.text = "Score: " + ScoreManager.CurrentScore;

        if (highScoreText != null)
            highScoreText.text = "Best: " + ScoreManager.HighScore;
    }
}