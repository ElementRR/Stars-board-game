using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPanel : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (GameObject.Find("GameManager").GetComponent<NetworkGM>())
        {
            NetworkGM.instance.OnFirstTurnEnd += HidePanel;
            NetworkGM.instance.OnShowTurnEnd += ShowPanel;
            NetworkUI.instance.OnHidePanel += HidePanel;
            NetworkUI.instance.OnShowPanel += ShowPanel;
        }
        else
        {
            GameManager.instance.OnFirstTurnEnd += HidePanel;
            GameManager.instance.OnShowTurnEnd += ShowPanel;
            UIManager.instance.OnHidePanel += HidePanel;
            UIManager.instance.OnShowPanel += ShowPanel;
        }

        animator.Play("CardsPanelOut", 0, 0.9f);
    }

    void HidePanel()
    {
        animator.SetTrigger("Hide");
    }
    void ShowPanel()
    {
        animator.SetTrigger("Show");
    }

    private void OnDestroy()
    {
        if (GameObject.Find("GameManager").GetComponent<NetworkGM>())
        {
            NetworkGM.instance.OnFirstTurnEnd -= HidePanel;
            NetworkGM.instance.OnShowTurnEnd -= ShowPanel;
            NetworkUI.instance.OnHidePanel -= HidePanel;
            NetworkUI.instance.OnShowPanel -= ShowPanel;
        }
        else
        {
            GameManager.instance.OnFirstTurnEnd -= HidePanel;
            GameManager.instance.OnShowTurnEnd -= ShowPanel;
            UIManager.instance.OnHidePanel -= HidePanel;
            UIManager.instance.OnShowPanel -= ShowPanel;
        }
    }
}
