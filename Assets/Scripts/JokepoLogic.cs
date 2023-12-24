using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JokepoLogic : MonoBehaviour
{
    public TextMeshProUGUI enemyText;

    public TextMeshProUGUI textDecision;

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

        if(isWinner) {

            textDecision.text = "You win";
        }
        else { textDecision.text = "You lose";
        }
    }
    private void ResetGame()
    {
        enemyHand = Random.Range(0, 2);
        isHandPicked = false;
    }
}
