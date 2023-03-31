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
        GameManager.instance.OnFirstTurnEnd += HidePanel;
        GameManager.instance.OnShowTurnEnd += ShowPanel;
    }

    void HidePanel()
    {
        animator.SetTrigger("Hide");
    }
    void ShowPanel()
    {
        animator.SetTrigger("Show");
    }
}
