using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimations : MonoBehaviour
{
    public static ButtonAnimations instance;
    public Animator[] buttons;

    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    public void EndActionT()
    {
        buttons[0].SetTrigger("ShowTurn");
        buttons[1].SetTrigger("ShowTurn");
    }
    public void EndShowT()
    {
        buttons[0].SetTrigger("ActionTurn");
        buttons[1].SetTrigger("ActionTurn");
    }
}
