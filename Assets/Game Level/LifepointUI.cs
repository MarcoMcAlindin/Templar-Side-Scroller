using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
public class LifepointUI : MonoBehaviour
{
    private Animator _animator;
    private readonly static string ANIM_TRNSITION = "Animate";

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnEnable()
    {
        _animator.SetTrigger(ANIM_TRNSITION);
    }
}
