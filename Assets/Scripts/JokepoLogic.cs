using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JokepoLogic : MonoBehaviour
{
    public TextMeshProUGUI enemyText;

    public TextMeshProUGUI textDecision;

    public WhoFirstLogic WhoFirstLogic;

    public int meHand;
    // 0 = rock, 1 = paper, 2 = scissors
    public int enemyHand;

    private bool isHandPicked = false;

    public bool isWinner;

    private void Awake()
    {
        ResetGame();
    }

    public void GetJokenpoGame(int number)
    {
        if (!isHandPicked)
        {
            meHand = number;
            isHandPicked = true;

            switch(enemyHand) 
            {
                case 0:
                    enemyText.text = "Rock";
                    break;
                case 1:
                    enemyText.text = "Paper";
                    break;
                case 2:
                    enemyText.text = "Scissors";
                    break;
                default:
                    enemyText.text = "Rock";
                    break;
            }

            GameDecision();
        }
    }
    public void GameDecision()
    {
        if (meHand == enemyHand)
        {
            textDecision.text = "It's a draw";
            ResetGame();
            return;
        }

        switch(meHand) 
        {
            case 0 when enemyHand == 1:
                isWinner = false;
                break;
            case 1 when enemyHand == 2:
                isWinner = false;
                break;
            case 2 when enemyHand == 0:
                isWinner = false;
                break;
            default:
                isWinner = true;
                break;
        }

        StartCoroutine(NextWindow());
    }
    private void ResetGame()
    {
        enemyHand = Random.Range(0, 2);
        isHandPicked = false;
    }

    private IEnumerator NextWindow() 
    {
        float time = 1.2f;

        if (isWinner)
        {
            textDecision.text = "You win";
            yield return new WaitForSeconds(time);
            Instantiate(WhoFirstLogic.gameObject, transform);
        }
        else
        {
            textDecision.text = "You lose";
            GameManager.instance.showFase1 = false;
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }

    }
}
