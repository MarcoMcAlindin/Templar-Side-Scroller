using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBatEnemy : EnemyAIController2D
{
    float _movementCounter;
    [SerializeField] float _swayAmount = 5.0f;

    public override void Start()
    {
        base.Start();

        _attackDelay = 0.0f;
        _attackRange = 1.0f;

        _healthSystem = new HealthSystem(1);

        _enemyPointValue = 150;
    }

    public override void Move()
    {
        base.Move();

        
        _movementCounter += 0.1f;
        _currentVelocity.y = Mathf.Sin(_movementCounter) * _swayAmount;
        
    }
}
