using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaAI : EnemyAI
{
    protected override void Awake()
    {
        showTurnAction = GetComponent<ShowTurnAction>();
        EnemyPlay();
        firstTurn = true;

        if (ScoreManager.instance._enemyName != Enemy.Name.Ana)
        {
            GetComponent<AnaAI>().enabled = false;
        }
    }
}
