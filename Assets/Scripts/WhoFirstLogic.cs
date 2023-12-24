using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoFirstLogic : MonoBehaviour
{
    public JokepoLogic jokenpoLogic;


    private void Awake()
    {
        jokenpoLogic = GetComponentInParent<JokepoLogic>();
    }
    public void GetPlayerChoice(bool isYou)
    {
        if (isYou)
        {
            jokenpoLogic.gameObject.SetActive(false);
            GameManager.instance.showFase1 = true;
        }
        else
        {
            jokenpoLogic.gameObject.SetActive(false);
            GameManager.instance.showFase1 = false;
        }
    }
}
