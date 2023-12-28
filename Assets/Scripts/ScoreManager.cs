using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy
{
    public enum Name { Ed, Rick, Ana };
}
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    public static int score;

    public Enemy.Name _enemyName;

    private void Start()
    {
        EnemySelection.OnEnemyChose += GetEnemyName;
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
        scoreText.text = "Your score: " + score;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Your score: " + score;
    }

    private void GetEnemyName(Enemy.Name enemyName)
    {
        _enemyName = enemyName;
    }
}
