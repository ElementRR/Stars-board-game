using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class NetworkJKP : AttributesSync
{
    public Image enemyImg;

    public Sprite[] enemyImgs;

    public Button[] meChoices;

    private Vector3 choiceFirstPos;

    public TextMeshProUGUI textDecision;

    [SerializeField] private GameObject waitingPlayer;

    [SerializeField] private GameObject whoFirst;

    [SynchronizableField][SerializeField] private int hostHand;
    // 0 = rock, 1 = paper, 2 = scissors
    [SynchronizableField][SerializeField] private int guestHand;

    private bool isHandPicked = false;

    [SynchronizableField] private bool isHostWinner;

    public delegate void Outdoor(string message);
    public static event Outdoor OnMessageSent;

    [Header("Sound FX")]
    protected AudioSource audioSource;
    [SerializeField] private AudioClip UIclick;
    [SerializeField] private AudioClip drawSound;

    [Header("Network")]
    private bool isHost;
    [SynchronizableField] int playersWaiting = 0;

    private void Awake()
    {
        Multiplayer.OnRoomJoined.AddListener(RoomJoined);

        waitingPlayer.SetActive(true);
        Time.timeScale = 0;
        audioSource = GetComponent<AudioSource>();
        ResetGame(false);
    }

    private void RoomJoined(Multiplayer multiplayer, Room room, User user)
    {
        isHost = (user.IsHost);

        if (Multiplayer.CurrentRoom.Users.Count > 1)
        {
            waitingPlayer.SetActive(false);
        }
    }

    public void GetJokenpoGame(int handIndex)
    {
        audioSource.PlayOneShot(UIclick);

        if (!isHandPicked)
        {
            isHandPicked = true;

            /*
            enemyImg.sprite = guestHand switch
            {
                0 => enemyImgs[0],
                1 => enemyImgs[1],
                2 => enemyImgs[2],
                _ => enemyImgs[0],
            };
            */

            meChoices[0].gameObject.SetActive(false); meChoices[1].gameObject.SetActive(false); meChoices[2].gameObject.SetActive(false);
            meChoices[handIndex].gameObject.SetActive(true);
            choiceFirstPos = meChoices[handIndex].gameObject.transform.localPosition;
            meChoices[handIndex].gameObject.transform.localPosition = meChoices[1].gameObject.transform.localPosition;
            
            waitingPlayer.SetActive(true);

            BroadcastRemoteMethod("WaitOtherPlayerJKP", handIndex);
        }
    }
    [SynchronizableMethod]
    void WaitOtherPlayerJKP(int handIndex)
    {
        if (isHost)
        {
            hostHand = handIndex;
        }
        else
        {
            guestHand = handIndex;
        }

        playersWaiting++;

        if (playersWaiting > 1)
        {
            waitingPlayer.SetActive(false);
            GameDecision();
        }
    }
    public void GameDecision()
    {
        if (hostHand == guestHand)
        {
            StartCoroutine(DrawResult());
            return;
        }

        isHostWinner = hostHand switch
        {
            0 when guestHand == 1 => false,
            1 when guestHand == 2 => false,
            2 when guestHand == 0 => false,
            _ => true,
        };
        StartCoroutine(NextWindow());
    }
    private void ResetGame(bool isDraw)
    {
        enemyImg.sprite = enemyImgs[enemyImgs.Length - 1];

        isHandPicked = false;

        if (isDraw)
        {
            int meHand;
            meHand = (isHost ? hostHand : guestHand);
            meChoices[meHand].gameObject.transform.localPosition = choiceFirstPos;

            meChoices[0].gameObject.SetActive(true); meChoices[1].gameObject.SetActive(true); meChoices[2].gameObject.SetActive(true);
        }
    }

    private IEnumerator NextWindow()
    {
        float time = 1.2f;

        yield return new WaitForSeconds(time);

        switch ((isHostWinner?0 : 1) + (isHost?0:2))
        {
            case 0 or 3:
                textDecision.text = "You win";
                break;
            case 1 or 2:
                textDecision.text = "You lose";
                break;
            default:
                break;
        }

        whoFirst.SetActive(true);
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

    public void WhoFirst(bool isYou)
    {
        audioSource.PlayOneShot(UIclick);

        if (isHost)
        {
            switch ((isYou ? 0 : 1) + (isHostWinner ? 0 : 2))
            {
                case 0:
                    GameManager.instance.showFase1 = true;
                    OnMessageSent?.Invoke(Multiplayer.CurrentRoom.Name + " will start showing cards!");
                    BroadcastRemoteMethod("StartMatchItself");
                    break;
                case 1:
                    GameManager.instance.showFase1 = false;
                    OnMessageSent?.Invoke(Multiplayer.CurrentRoom.Users.Last().Name + " will start to show cards!");
                    BroadcastRemoteMethod("StartMatchItself");
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch ((isYou ? 0 : 1) + (isHostWinner ? 0 : 2))
            {
                case 3:
                    GameManager.instance.showFase1 = true;
                    OnMessageSent?.Invoke(Multiplayer.CurrentRoom.Name + " will start showing cards!");
                    BroadcastRemoteMethod("StartMatchItself");
                    break;
                case 2:
                    GameManager.instance.showFase1 = false;
                    OnMessageSent?.Invoke(Multiplayer.CurrentRoom.Users.Last().Name + " will start to show cards!");
                    BroadcastRemoteMethod("StartMatchItself");
                    break;
                default:
                    break;
            }
        }
    }

    [SynchronizableMethod]
    void StartMatchItself()
    {
        NetworkGM.instance.StartGame();
        gameObject.SetActive(false);
    }

}
