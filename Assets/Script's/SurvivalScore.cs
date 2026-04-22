using UnityEngine;

public class SurvivalScore : MonoBehaviour
{
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 60f)
        {
            ScoreManager.AddScore(1000);
            timer = 0f;
        }
    }

    public void OnLevelUp()
    {
        ScoreManager.AddScore(50);
    }

    public void OnEnemyKilled()
    {
        ScoreManager.AddScore(30);
    }
}