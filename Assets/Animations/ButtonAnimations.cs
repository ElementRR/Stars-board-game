using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimations : MonoBehaviour
{
    public Animator[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.actionTurn)
        {
            buttons[0].SetTrigger("ActionTurn");
            buttons[1].SetTrigger("ActionTurn");
        }
        else
        {
            buttons[0].SetTrigger("ShowTurn");
            buttons[1].SetTrigger("ShowTurn");
        }
    }
}
