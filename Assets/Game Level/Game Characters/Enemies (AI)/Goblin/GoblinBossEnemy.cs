using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBossEnemy : EnemyAIController2D
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        _attackRange = 100.0f;
        _attackDelay = 5.0f;
        _noticeTime = 7.0f;
        _enemyPointValue = 1000;

        _healthSystem = new HealthSystem(5);
    }

    public override bool IsCharacterInRange(LayerMask characterLayer)
    {
        RaycastHit2D raycast = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), _direction, _attackRange, characterLayer);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), _direction * _attackRange, Color.blue);

        if (raycast.collider != null && !_hasNoticedEnemy)
        {
            _noticeTime = Time.time;
            _hasNoticedEnemy = true;
        }
        else if (raycast.collider == null)
        {
            _hasNoticedEnemy = false;
        }

        return raycast.collider != null;
    }
}
