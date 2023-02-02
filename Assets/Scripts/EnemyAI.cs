using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.instance.slot4card = 3;
        UIManager.instance.slot5card = 4;
        UIManager.instance.slot6card = 5;

        GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot4);
        GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot5);
        GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot6);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
