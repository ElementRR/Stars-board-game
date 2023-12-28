using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GetScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    void Awake()
    {
        scoreText.text = "Your score: " + ScoreManager.score;
    }
}
