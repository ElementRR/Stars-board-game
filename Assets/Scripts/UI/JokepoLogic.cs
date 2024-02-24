using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JokepoLogic : MonoBehaviour
{
    public Image enemyImg;

    public Sprite[] enemyImgs;

    public Button[] meChoices;

    private Vector3 choiceFirstPos;

    public TextMeshProUGUI textDecision;

    public WhoFirstLogic WhoFirstLogic;

    public int meHand;
    // 0 = rock, 1 = paper, 2 = scissors
    public int enemyHand;

    private bool isHandPicked = false;

    public bool isWinner;

    [Header("Sound FX")]
    protected AudioSource audioSource;
    [SerializeField] private AudioClip UIclick;
    [SerializeField] private AudioClip drawSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ResetGame(false);
    }

    public void GetJokenpoGame(int number)
    {
        audioSource.PlayOneShot(UIclick);

        if (!isHandPicked)
        {
            meHand = number;
            isHandPicked = true;

            enemyImg.sprite = enemyHand switch
            {
                0 => enemyImgs[0],
                1 => enemyImgs[1],
                2 => enemyImgs[2],
                _ => enemyImgs[0],
            };

            meChoices[0].gameObject.SetActive(false); meChoices[1].gameObject.SetActive(false); meChoices[2].gameObject.SetActive(false);
            meChoices[meHand].gameObject.SetActive(true);
            choiceFirstPos = meChoices[meHand].gameObject.transform.localPosition;
            meChoices[meHand].gameObject.transform.localPosition = meChoices[1].gameObject.transform.localPosition;

            GameDecision();
        }
    }
    public void GameDecision()
    {
        if (meHand == enemyHand)
        {
            StartCoroutine(DrawResult());
            return;
        }

        isWinner = meHand switch
        {
            0 when enemyHand == 1 => false,
            1 when enemyHand == 2 => false,
            2 when enemyHand == 0 => false,
            _ => true,
        };
        StartCoroutine(NextWindow());
    }
    private void ResetGame(bool isDraw)
    {
        enemyImg.sprite = enemyImgs[enemyImgs.Length - 1];
        enemyHand = Random.Range(0, 2);
        isHandPicked = false;

        if (isDraw)
        {
            meChoices[meHand].gameObject.transform.localPosition = choiceFirstPos;
            meChoices[0].gameObject.SetActive(true); meChoices[1].gameObject.SetActive(true); meChoices[2].gameObject.SetActive(true);
        }
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
            Instantiate(WhoFirstLogic.gameObject, transform);
            //gameObject.SetActive(false);
        }

    }
    private IEnumerator DrawResult()
    {
        float time = 1.2f;

        audioSource.PlayOneShot(drawSound);
        textDecision.text = "Draw";
        yield return new WaitForSeconds(time);
        textDecision.text = "Chose another";
        ResetGame(true);
    }
}
