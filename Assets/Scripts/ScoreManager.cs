using TMPro;
using UnityEngine;

public class Enemy
{
    public enum Name { Ed, Rick, Ana };
}
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public static int score;

    public static bool isEdWon = false;

    public Enemy.Name _enemyName;

    private void Start()
    {
        EnemySelection.OnEnemyChose += GetEnemyName;

        score = PlayerPrefs.GetInt("score");

        int intToBool = 0;
        intToBool = PlayerPrefs.GetInt("isEdWon");
        isEdWon = (intToBool == 1); 
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance = null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        PlayerPrefs.SetInt("score", score);
    }

    private void GetEnemyName(Enemy.Name enemyName)
    {
        _enemyName = enemyName;
    }
}
