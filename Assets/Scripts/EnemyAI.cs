using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private void Start()
    {
        EnemyPlay();
    }
    public void EnemyPlay()
    {
        if (GameManager.instance.actionTurn)
        {
            UIManager.instance.slot4card = Random.Range(0, 6);
            UIManager.instance.slot5card = Random.Range(0, 6);
            UIManager.instance.slot6card = Random.Range(0, 6);

                GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot4, true);
                GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot5, true);
                GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot6, true);
            
        }
    }
}
