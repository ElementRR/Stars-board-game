using TMPro;
using UnityEngine;

public class Enemy
{
    public enum Name { Ed, Rick, Ana };
}
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] public static int score;

    [SerializeField] public static bool isEdWon = false;

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
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    private void GetEnemyName(Enemy.Name enemyName)
    {
        _enemyName = enemyName;
    }
}
