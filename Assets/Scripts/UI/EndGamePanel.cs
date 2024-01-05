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
                Enemy.Name.Ed => "Ed: I believe something is off... Did you let me win?",
                Enemy.Name.Rick => "Rick: I was a nice game. But I've been training for some time\nmaybe next time? ",
                Enemy.Name.Ana => "Ana: HAHAHA Did you really think you could win?\nA loser like you will never beat a genius like me!",
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
                Enemy.Name.Ed => "Ed: Oh fine. It was a nice game! :(\n I wish you good luck with Rick",
                Enemy.Name.Rick => "Rick: Hey, how could this happen?\nDid you cheat o something?\nC-congrats!",
                Enemy.Name.Ana => "Ana: NO! This is not possible!!!\nI've been training for a long time!\nI'm a genius! Get back here!",
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
