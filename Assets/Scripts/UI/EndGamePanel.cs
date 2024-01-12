using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject[] announce;
    [SerializeField] private TextMeshProUGUI pointsEarned;
    [SerializeField] private TextMeshProUGUI finalText;
    [SerializeField] private GameObject angryAnaBt;

    [SerializeField] Animator blackPanel;

    private int points;

    public delegate void Outdoor(Enemy.Name enemyName);
    public static event Outdoor OnEnemyChose;

    private void Awake()
    {
        angryAnaBt.SetActive(false);
    }

    public void GameResult(bool enemyWins)
    {
        Enemy.Name enemyIndex;
        enemyIndex = GameManager.enemyIndex;

        if (enemyWins)
        {
            announce[1].SetActive(true);
            announce[0].SetActive(false);

            points = enemyIndex switch
            {
                Enemy.Name.Ed => 0,
                Enemy.Name.Rick => 50,
                Enemy.Name.Ana => 50,
                Enemy.Name.AngryAna => 50,
                _ => 0,
            };

            finalText.text = enemyIndex switch
            {
                Enemy.Name.Ed => "Ed: I believe something is off...\nDid you let me win?",
                Enemy.Name.Rick => "Rick: I've been training for a while now, you couldn't win like that...\nMaybe another day?\nIt was a nice game though.",
                Enemy.Name.Ana => "Ana: HAHAHA Did you really think you could win?\nA loser like you will never beat a genius like me!",
                Enemy.Name.AngryAna => "Ana: HAHAHA I imagine you had beginner's luck before!\nYou will never beat me.\nTry another day.",
                _ => "Error",
            };
        }
        else
        {
            announce[0].SetActive(true);
            announce[1].SetActive(false);

            points = enemyIndex switch
            {
                Enemy.Name.Ed => 50,
                Enemy.Name.Rick => 100,
                Enemy.Name.Ana => 200,
                Enemy.Name.AngryAna => 250,
                _ => 0,
            };

            finalText.text = enemyIndex switch
            {
                Enemy.Name.Ed => "Ed: Oh fine. It was a nice game! :(\n I wish you good luck with the other players",
                Enemy.Name.Rick => "Rick: Hey, how could this happen?\nDid you cheat or something?\nC-congrats!",
                Enemy.Name.Ana => "Ana: NO! This is not possible!!!\nI've been training for a long time!\nI'm a genius! Get back here for another round!",
                Enemy.Name.AngryAna => "Ana: W-wait... How could you win against me?\nThat's not possible, I had an advantage... I-I mean..." +
                "\nI don't believe you was able to beat me under these circumstances.",
                _ => "Error",
            };

            if(enemyIndex == Enemy.Name.Ed)
            {
                ScoreManager.isEdWon = true;
                PlayerPrefs.SetInt("isEdWon", 1);
            }else if (enemyIndex == Enemy.Name.Ana) angryAnaBt.SetActive(true);
        }

        pointsEarned.text = "+ " + points;
        ScoreManager.instance.AddScore(points);
    }

    public void OnAngryBtClick()
    {
        OnEnemyChose?.Invoke(Enemy.Name.AngryAna);
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        blackPanel.Play("TransToBlack");
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(1);
    }
}
