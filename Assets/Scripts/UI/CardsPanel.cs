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
        //animator.SetTrigger("Hide");
        animator.SetBool("isShowing", false);
    }
    void ShowPanel()
    {
        //animator.SetTrigger("Show");
        animator.SetBool("isShowing", true);
    }

    private void OnDestroy()
    {
        if (GameObject.Find("GameManager").TryGetComponent(out NetworkGM net))
        {
            net.OnFirstTurnEnd -= HidePanel;
            net.OnShowTurnEnd -= ShowPanel;
            NetworkUI.instance.OnHidePanel -= HidePanel;
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
