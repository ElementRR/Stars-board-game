using UnityEngine;

public class WhoFirstLogic : MonoBehaviour
{
    public JokepoLogic jokenpoLogic;

    public delegate void Outdoor(string message);
    public static event Outdoor OnMessageSent;

    private void Awake()
    {
        jokenpoLogic = GetComponentInParent<JokepoLogic>();
        if(!GameManager.instance.showFase1)
        {
            OnMessageSent?.Invoke("Enemy will start showing cards!!");
            jokenpoLogic.gameObject.SetActive(false);
        }
    }
    public void GetPlayerChoice(bool isYou)
    {
        if (isYou)
        {
            GameManager.instance.showFase1 = true;
            OnMessageSent?.Invoke("You will start showing cards!");
            jokenpoLogic.gameObject.SetActive(false);
        }
        else
        {
            GameManager.instance.showFase1 = false;
            OnMessageSent?.Invoke("Enemy will start to show cards!");
            jokenpoLogic.gameObject.SetActive(false);
        }
    }

}
