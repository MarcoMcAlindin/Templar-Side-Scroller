using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(BoxCollider2D))]
public class Bullet : MonoBehaviour
{
    //VISIBLE IN EDITOR
    [SerializeField] float _bulletForce;
    [SerializeField] Vector2 _bulletDirection;

    //Components 
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider2D;

    public bool _hasCollided = false;

    float _enabledTime;
    float _destroyTime = 3.0f;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        Physics2D.IgnoreLayerCollision(LayerMask.GetMask("Prop"), LayerMask.GetMask("Bullet"));
    }

    public void SpawnBullet(Vector2 direction)
    {
        _bulletForce = 20.0f;

        _bulletDirection = direction;

        _rigidbody.velocity = _bulletDirection * _bulletForce;
    }

    private void OnEnable()
    {
        _enabledTime = Time.time;
    }

    public void Update()
    {
        if (Time.time > _enabledTime + _destroyTime)
        {
            ObjectPool.Instance.PoolObject(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectPool.Instance.PoolObject(this.gameObject);
    }

  
}
