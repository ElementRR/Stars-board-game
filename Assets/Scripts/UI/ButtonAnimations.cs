using UnityEngine;

public class ButtonAnimations : MonoBehaviour
{
    public Animator[] buttons;

    private void Start()
    {
        if (GameObject.Find("GameManager").GetComponent<NetworkGM>())
        {
            NetworkGM.instance.OnFirstTurnEnd += EndActionT;
            NetworkGM.instance.OnShowTurnEnd += EndShowT;
        }
        else
        {
            GameManager.instance.OnFirstTurnEnd += EndActionT;
            GameManager.instance.OnShowTurnEnd += EndShowT;
        }
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
