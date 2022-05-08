using UnityEngine;
using System.Collections;


public class GameCharacterController2D : GameCharacter2DBase
{
    //Input Actions Asset
    private TemplarSideScroller _inputActions;

    //Jumping 
    [Header("Jump")]
    [SerializeField] protected bool _isJumping = false;
    [SerializeField] protected bool _isGrounded = true;
    protected float _jumpStartPosition;

    [SerializeField] protected float _jumpHeight;
    [SerializeField] protected float _jumpForce = 25.0f;


    [SerializeField] public bool _canShoot = false;
    [SerializeField] public bool _isInvincible = false;

    bool _playerHurt = false;

    [SerializeField] Sprite _jumpSprite;
    [SerializeField] Sprite _fallSprite;

    [SerializeField] public GameObject _touchControlsUI;

    [SerializeField] MobileButton _leftButton;
    [SerializeField] MobileButton _rightButton;
    [SerializeField] MobileButton _attackButton;
    [SerializeField] MobileButton _jumpButton;

    public static event CharacterEvent OnCharacterHurt;
    public static event CharacterEvent OnPlayerDead;


    Vector2 _initialPosition;

    private void OnEnable()
    {
        //Invincible 
        InvinciblePickUp.OnInvinciblePickUp += InvincibleSpeed;

        //Extra lifepoints
        HeartPickup.OnHeartPickUp += _healthSystem.AddOneHealth;

        GameManager.OnGameRestart += ResetToLevelStart;
    }

    private void OnDisable()
    {
        //Invincible 
        InvinciblePickUp.OnInvinciblePickUp -= InvincibleSpeed;

        //Extra lifepoints
        HeartPickup.OnHeartPickUp -= _healthSystem.AddOneHealth;

        GameManager.OnGameRestart -= ResetToLevelStart;

    }



    public void Start()
    {
        _initialPosition = transform.position;

        //Construct Input Asset class
        _inputActions = new TemplarSideScroller();
        _inputActions.Enable();

        //Health system initialisation
        _healthSystem = new HealthSystem(3, 2);

        //Health System UI Initialisation, 
        UIManager.Instance.InitialiseLifepointUI();



        if (GameManager.Instance.IsMobilePlatform())
        {
            _touchControlsUI.SetActive(true);
        }
        else
        {
            _touchControlsUI.SetActive(false);
        }

        UIManager.Instance.HidePauseMenu();

    }

    public override void FixedUpdate()
    {

        if (!GameManager.Instance._isGamePaused)
        {
            //Jump if var is set to true
            if (_isJumping)
            {
                Jump();
            }

            if (IsGrounded())
            {
                _animator.enabled = true;
            }
            else
            {
                _isGrounded = false;
            }




            base.FixedUpdate();
        }
    }
    public void Update()
    {

        if (!GameManager.Instance._isGamePaused)
        {
            ProcessInput();

            if (HasJumpedOnEnemy())
            {
                _currentTarget.GetComponent<EnemyAIController2D>().GetHealthSystem().TakeDamage(1);

                _jumpStartPosition = transform.position.y;

                _isJumping = true;
            }

            if (transform.position.y < -15.0f) { GameManager.Instance._gameOver = true; }
        }
    }

    //Check if character is hitting head on platform
    protected bool IsHittingHead()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.up, 1.0f, _platformLayerMask);

        return raycast.collider != null;
    }


    public override void Attack()
    {
        base.Attack();

        if (IsCharacterInRange(_enemyLayerMask))
        {
            _currentTarget.GetComponent<EnemyAIController2D>()._isHurt = true;
        }
    }

    public override IEnumerator Die()
    {
        _animator.SetTrigger(ANIM_DEAD);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance._gameOver = true;

        OnPlayerDead?.Invoke();
    }


    //Jump Method
    public void Jump()
    {

        //Set grounded to false
        _isGrounded = false;

        if (transform.position.y > _jumpStartPosition + _jumpHeight || IsHittingHead())
        {
            //Set velocity on y axis to zero
            _currentVelocity.y = 0.0f;

            //Set falling Sprite
            _spriteRenderer.sprite = _fallSprite;

            _isJumping = false;
        }
        else
        {
            //Set y velocity to jump force
            _currentVelocity.y = _jumpForce;

            if (!_isAttacking)
            {
                //Deactivate animator in order to change sprite to jumping
                _animator.enabled = false;

                //Set jumping sprite
                _spriteRenderer.sprite = _jumpSprite;

            }
        }


    }

    public override bool IsCharacterInRange(LayerMask _characterLayer)
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, _direction, _attackRange, _characterLayer);
        Debug.DrawRay(transform.position, _direction * _attackRange, Color.red);


        if (raycast.collider != null) { _currentTarget = raycast.collider.gameObject.transform; }


        return raycast.collider != null;
    }

    //Process player input
    public void ProcessInput()
    {
        //Check if Jump input button has been pressed 
        if (_inputActions.Player.Jump.WasPressedThisFrame())
        {
            //Assign jump start position and jumping variable if grounded
            if (_isGrounded)
            {
                _jumpStartPosition = transform.position.y;

                _isJumping = true;
            }
        }

        //Check if Attack button has been pressed and set attacking to true
        if (_inputActions.Player.Attack.WasPerformedThisFrame()) { _isAttacking = true; }

        if (_inputActions.Player.Move.IsPressed())
        {
            _isMoving = true;

            //Read input from x axis and assign to direction
            _direction.x = _inputActions.Player.Move.ReadValue<Vector2>().x;
        }
        else
        {
            _isMoving = false;
        }

#if !UNITY_EDITOR
         if (_leftButton._buttonPressed) { _isMoving = true; _direction.x = -1; }
            else if (_rightButton._buttonPressed) { _isMoving = true; _direction.x = 1; }
            else { _isMoving = false; }

        if(_jumpButton._buttonPressed) { SetJumpToTrue(); _jumpButton._buttonPressed = false;}
        if(_attackButton._buttonPressed){SetAttackToTrue(); _attackButton._buttonPressed = false; }
#endif



    }


    //Check if Player has jumped on top of an enemy
    public bool HasJumpedOnEnemy()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(_boxCollider.bounds.center, new Vector2(_boxCollider.bounds.size.x, _boxCollider.bounds.size.y / 2), 0f, Vector2.down, 1.0f, _enemyLayerMask);

        if (raycast.collider != null)
        {
            _currentTarget = raycast.collider.gameObject.transform;

            raycast.collider.gameObject.GetComponent<EnemyAIController2D>()._isHurt = true;

        }

        return raycast.collider != null;
    }

    //Flip Player Sprite in appropriate direction
    public override void FlipSpriteInDirection()
    {

        if (_currentVelocity.x > 0) _spriteRenderer.flipX = false; else if (_currentVelocity.x < 0) _spriteRenderer.flipX = true;

    }

    //Check if character is on ground
    protected bool IsGrounded()
    {
        RaycastHit2D raycast = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, 1.0f, _platformLayerMask);

        _isGrounded = true;

        return raycast.collider != null;
    }

    public void InvincibleSpeed()
    {
        _movementSpeed = 30.0f;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickup")
        {
            collision.GetComponent<PickupScript>().ActivatePickupEffect();
        }

        if (collision.gameObject.tag == "Bullet")
        {
            _isHurt = true;

            ObjectPool.Instance.PoolObject(collision.gameObject);
        }

        if (collision.gameObject.tag == "Death Collider")
        {
            _healthSystem.TakeDamage(_healthSystem.GetCurrentHealth());
            UIManager.Instance.RemoveAllLifepointsUI();
        }

        if (collision.gameObject.tag == "Game Start Trigger")
        {
            GameManager.Instance._isGameStarted = true;
        }
    }

    //For mobile controls
    public void SetMovementToTrue()
    {
        _isMoving = true;
    }

    public void SetAttackToTrue()
    {
        _isAttacking = true;
    }

    public void SetJumpToTrue()
    {
        //Assign jump start position and jumping variable if grounded
        if (_isGrounded)
        {
            _jumpStartPosition = transform.position.y;

            _isJumping = true;
        }
    }


    public override void Hurt()
    {
        base.Hurt();

        OnCharacterHurt?.Invoke();

    }

    void ResetToLevelStart()
    {

        _healthSystem = new HealthSystem(2,3);

        _isDead = false;

        transform.position = new Vector2(_initialPosition.x, _initialPosition.y + 0.1f);

        GameManager.Instance._gameOver = false;
    }

}
