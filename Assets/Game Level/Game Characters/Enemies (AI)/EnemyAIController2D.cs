using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController2D : GameCharacter2DBase
{
    //VISIBLE IN EDITOR
    [Header("AGENT AI", order = 0)]

    [Header("Target", order = 1)]
    [SerializeField] protected bool _chaseTarget = false;

    [Header("Attack Values")]
    [SerializeField] protected float _attackDelay = 0.5f;

    [SerializeField] private float _lastAttackTime;
    [SerializeField] private float _attackTimer;

    [SerializeField] protected float _reactionTime = .5f;
    [SerializeField] protected float _noticeTime;
    [SerializeField] private float _reactionTimer;

    [SerializeField] protected int _enemyPointValue;
    float _turnAroundDistance = 6.0f;
    protected bool _hasNoticedEnemy = false;

    //--------------------------


    bool _isFacingTarget = true;
    bool _isHittingWall = false;


    public virtual void Start()
    {
        //Assign target variable with player
        _currentTarget = GameObject.FindGameObjectWithTag("Player").transform;

        _platformLayerMask = LayerMask.GetMask("Platform");

        _enemyLayerMask = LayerMask.GetMask("Player");

        //Set Collision between enemies to be ignored
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);

        //Set Collision between enemies to be ignored
        Physics2D.IgnoreLayerCollision(gameObject.layer, _platformLayerMask);
    }

    public void Update()
    {
        if (!GameManager.Instance._isGamePaused)
        {
            //Attack Execution-> Check if the target is in range using enemy layer and check if agent can attack, if true execute functions in scope
            if (IsCharacterInRange(_enemyLayerMask) && CanAttack())
            {
                if(Time.time > _noticeTime + _reactionTime)
                {
                    //Base class Attack Method
                    Attack();

                    //Assign the current time and reset attack timer
                    _lastAttackTime = Time.time;
                    _attackTimer = 0.0f;
                }
                else
                {
                    _reactionTimer += Time.deltaTime;
                }
               

                
            }
            else
            {
                //Increment attack timer 
                _attackTimer += Time.deltaTime;
            }

            //Execute methods in scope if Agent is set to chase target
            if (_chaseTarget)
            {
                //Turn towards target if needed
                TurnAroundTowardsTarget();
            }

            //If Agent is about to hit a wall change it's direction
            if (IsHittingWall())
            {
                _direction *= -1;
            }

            if (_currentTarget != null) if (Vector2.Distance(_currentTarget.position, transform.position) < 45.0f) { _isMoving = true; }
        }
        
    }

    //Checks if enough time has passed and return true if AI Agent can attack again
    bool CanAttack()
    {
        if (Time.time > _lastAttackTime + _attackDelay && !_isHurt)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Turns Agent towards current target if needed
    public void TurnAroundTowardsTarget()
    {
        if (_currentTarget != null)
        {
            //Based on current character direction checks if AI agent is facing current target
            if (_direction == Vector2.left)
            {
                if (transform.position.x < _currentTarget.transform.position.x - _turnAroundDistance)
                {
                    _isFacingTarget = false;
                }
            }
            else if (_direction == Vector2.right)
            {
                if (transform.position.x > _currentTarget.transform.position.x + _turnAroundDistance)
                {
                    _isFacingTarget = false;
                }
            }

            //If the agent is not facing the target change direction and assign true to facing boolean variable
            if (!_isFacingTarget)
            {
                _direction.x *= -1;
                _isFacingTarget = true;
            }
        }
       
    }

    //Check with raycast if Agent is about to hit a wall
    protected bool IsHittingWall()
    {

        RaycastHit2D raycast = Physics2D.Raycast(transform.position, _direction, 2.0f, _platformLayerMask);

        return raycast.collider != null;
    }

    //Override of abstract method-> Flips the active sprite towards using current direction
    public override void FlipSpriteInDirection()
    {

        if (_currentVelocity.x < 0) _spriteRenderer.flipX = false; else if (_currentVelocity.x > 0) _spriteRenderer.flipX = true;

    }

    public override bool IsCharacterInRange(LayerMask characterLayer)
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, _direction, _attackRange, characterLayer);
        Debug.DrawRay(transform.position, _direction * _attackRange, Color.blue);

        if (raycast.collider != null && !_hasNoticedEnemy)
        {
            _noticeTime = Time.time;
            _hasNoticedEnemy = true;
        }
        else if(raycast.collider == null)
        {
            _hasNoticedEnemy = false;
        }

        return raycast.collider != null;
    }

    public override void Attack()
    {
        base.Attack();

        _currentTarget.GetComponent<GameCharacterController2D>()._isHurt = true; ;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LevelEdgeCollider") { Destroy(this.gameObject); }
    }

    public override IEnumerator Die()
    {
        if (GameManager._scoreUpdated)
        {
            GameManager._playerScore += _enemyPointValue;
            GameManager._scoreUpdated = false;
        }
       

        return base.Die();

    }
}
