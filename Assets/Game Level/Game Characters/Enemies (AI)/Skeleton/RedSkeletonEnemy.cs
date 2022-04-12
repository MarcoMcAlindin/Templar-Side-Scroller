using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSkeletonEnemy : EnemyAIController2D
{
    public override void Start()
    {
        base.Start();

        _enemyPointValue = 250;
    }
}
