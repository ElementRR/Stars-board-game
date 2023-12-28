using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    public static int score;

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
}
