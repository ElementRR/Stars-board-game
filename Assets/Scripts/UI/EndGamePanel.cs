using TMPro;
using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject[] announce;
    [SerializeField] private TextMeshProUGUI pointsEarned;
    [SerializeField] private TextMeshProUGUI finalText;

    private int points;

    public void GameResult(bool enemyWins)
    {
        if (enemyWins)
        {
            announce[1].SetActive(true);
            announce[0].SetActive(false);

            points = GameManager.instance.enemyIndex switch
            {
                Enemy.Name.Ed => 0,
                Enemy.Name.Rick => 50,
                Enemy.Name.Ana => 100,
                _ => 0,
            };

            finalText.text = GameManager.instance.enemyIndex switch
            {
                Enemy.Name.Ed => "Ed won",
                Enemy.Name.Rick => "Rick won",
                Enemy.Name.Ana => "Ana won",
                _ => "Error",
            };
        }
        else
        {
            announce[0].SetActive(true);
            announce[1].SetActive(false);

            points = GameManager.instance.enemyIndex switch
            {
                Enemy.Name.Ed => 50,
                Enemy.Name.Rick => 100,
                Enemy.Name.Ana => 200,
                _ => 0,
            };

            finalText.text = GameManager.instance.enemyIndex switch
            {
                Enemy.Name.Ed => "Ed wish you a good luck with Rick",
                Enemy.Name.Rick => "Rick is angry, he wants Ana to destroy you",
                Enemy.Name.Ana => "Ana is angry",
                _ => "Error",
            };

            if(GameManager.instance.enemyIndex == Enemy.Name.Ed)
            {
                ScoreManager.isEdWon = true;
                PlayerPrefs.SetInt("isEdWon", 1);
            }
        }

        pointsEarned.text = "+ " + points;
        ScoreManager.instance.AddScore(points);
    }

}
