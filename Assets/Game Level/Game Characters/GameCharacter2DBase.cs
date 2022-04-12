using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
public abstract class GameCharacter2DBase: MonoBehaviour
{
   
    //VISIBLE IN EDITOR
    [SerializeField] public string _characterName;

    //Movement
    [Header("Locomotion")]

    [SerializeField] public bool _isMoving = false;
    [SerializeField] protected float _movementSpeed;

    [SerializeField] protected Vector2 _currentVelocity;
    [SerializeField] protected Vector2 _direction;

    //Attacking
    [Header("Attacking")]

    [SerializeField] protected Transform _currentTarget;
    [SerializeField] protected bool _isAttacking = false;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected LayerMask _enemyLayerMask;

    [Header("Health System")]
    [SerializeField] protected HealthSystem _healthSystem;
    public HealthSystem GetHealthSystem() { return _healthSystem; }
     public bool _isHurt = false;
    [SerializeField] GameObject _bloodVFX;

    [SerializeField] protected LayerMask _platformLayerMask;

    private int _animatorHash;

    //------------------------------------------------

    protected bool _isDead = false;
    

    //Components
    protected Rigidbody2D _rigidbody;
    protected Animator _animator;
    protected BoxCollider2D _boxCollider;
    protected SpriteRenderer _spriteRenderer;

    //Animator Parameters
    protected static readonly string ANIM_MOVING = "IsMoving";
    protected static readonly string ANIM_ATTACKING = "IsAttacking";
    protected static readonly string ANIM_JUMP_ATTACKING = "IsJumpAttacking";
    protected static readonly string ANIM_DEAD = "IsDead";
    protected static readonly string ANIM_HURT = "IsHurt";

    //EVENTS
    public delegate void CharacterEvent();
    public static event CharacterEvent OnCharacterDead;
    public static event CharacterEvent OnCharacterAttack;

    //ABSTRACT METHODS
    public abstract void FlipSpriteInDirection();
    public abstract bool IsCharacterInRange(LayerMask characterLayer);

    //ENGINE METHODS
    public void Awake()
    {
        //Assign character name with gameobject name
        _characterName = gameObject.name;

        //Assign component variables
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public  virtual void FixedUpdate()
    {
        if (!GameManager.Instance._isGamePaused)
        {
            //Check if Character has been hurt and execute method
            if (_isHurt) { Hurt(); }

            //Check if Character is dead and execute method
            if (_isDead) { StartCoroutine(Die()); }

            //Check if Character is attacking and execute method
            if (_isAttacking) { Attack(); }

            //Set dead variable to true if no health left, otherwise check if player is in movement and execute methods accordingly
            if (_healthSystem.HasNoHealthLeft())
            {
                _isDead = true;
                _currentVelocity = Vector2.zero;
            }
            else if (_isMoving)
            {
                Move();
            }
            else
            {
                _animatorHash = Animator.StringToHash(ANIM_MOVING);

                _animator.SetBool(_animatorHash, false);
                _currentVelocity.x = 0.0f;
            }
           
            //Set current velocity calculated in this frame to the rigidbodies velocity
            _rigidbody.velocity = _currentVelocity;
        }      
    }

    //-------------------

    //Move 
    public  virtual void Move()
    {
        //Set animator parameter
        _currentVelocity.x = _direction.x * _movementSpeed;

        if (_currentVelocity.x != 0)
        {
            _animatorHash = Animator.StringToHash(ANIM_MOVING);
            _animator.SetBool(_animatorHash, true);

        }

        FlipSpriteInDirection();
    }
 
    //Attack Method
    public virtual void Attack()
    {
        OnCharacterAttack?.Invoke();

        //Set animator trigger
        _animatorHash = Animator.StringToHash(ANIM_ATTACKING);
        _animator.SetTrigger(_animatorHash);

        //Set attacking variable to true
        _isAttacking = false;   
    }

    //Hurt Method
    public virtual void Hurt()
    {
        _isHurt = false;

        _animator.SetTrigger(ANIM_HURT);

        _healthSystem.TakeOneDamage();

        GameObject _bloodEffect = Instantiate(_bloodVFX, transform.position, Quaternion.identity);
        _bloodEffect.transform.parent = this.gameObject.transform;

        _animatorHash = Animator.StringToHash("Animate");

        _bloodEffect.GetComponent<Animator>().SetTrigger(_animatorHash);

        Destroy(_bloodEffect, 0.4f);

    }

    //Die Method
    public virtual IEnumerator Die()
    {

        yield return new WaitForSeconds(0.1f);

        _animatorHash = Animator.StringToHash(ANIM_DEAD);
        _animator.SetTrigger(_animatorHash);

        

        Destroy(this.gameObject, 0.5f);
    }
   
}
