using UnityEngine;
using UnityEngine.SceneManagement;

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

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetCurrentScore();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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