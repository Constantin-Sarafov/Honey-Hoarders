using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public static int CurrentScore { get; private set; }
    public static int HighScore { get; private set; }

    public static event System.Action OnScoreUpdated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public static void AddScore(int points)
    {
        CurrentScore += points;

        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt("HighScore", HighScore);
            PlayerPrefs.Save();
        }

        OnScoreUpdated?.Invoke();
    }

    public static void ResetCurrentScore()
    {
        CurrentScore = 0;
        OnScoreUpdated?.Invoke();
    }
}