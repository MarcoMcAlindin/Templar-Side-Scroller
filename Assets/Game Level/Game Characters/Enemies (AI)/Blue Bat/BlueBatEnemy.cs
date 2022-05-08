using UnityEngine;

public class BlueBatEnemy : EnemyAIController2D
{
    [SerializeField] GameObject _bulletPrefab;


    public override void Start()
    {
        base.Start();

        _attackDelay = 1.75f;
        _attackRange = 30.0f;


        _enemyPointValue = 100;

    }

    public override void Attack()
    {
        //Set animator trigger
        _animator.SetTrigger(ANIM_ATTACKING);

        //Set attacking variable to true
        _isAttacking = false;

        GameObject tempObj = ObjectPool.Instance.PullObject("Bullet Prefab");

        tempObj.transform.position = transform.position + new Vector3(_direction.x * 3.0f, -0.5f, 0.0f);

        tempObj.GetComponent<Bullet>().SpawnBullet(_direction);
    }

   
}
