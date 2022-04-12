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

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();        
    }

    public void SpawnBullet(Vector2 direction)
    {
        
        _bulletForce = 20.0f;

        _bulletDirection = direction;

        _rigidbody.velocity = _bulletDirection * _bulletForce;

        //Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.GetMask(6.ToString()));

        Destroy(this.gameObject, 5.0f);
    }

    private void OnEnable()
    {
        
    }

    public void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {     
        Destroy(this.gameObject);
    }
}
