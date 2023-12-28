using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RickAI : EnemyAI
{
    protected override void Awake()
    {
        showTurnAction = GetComponent<ShowTurnAction>();
        EnemyPlay();
        firstTurn = true;

        if (ScoreManager.instance._enemyName != Enemy.Name.Rick)
        {
            GetComponent<RickAI>().enabled = false;
        }
    }
}
