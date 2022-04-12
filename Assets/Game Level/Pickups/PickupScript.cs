using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupScript : MonoBehaviour
{
    public delegate void PickUpAction();

    protected GameCharacterController2D _playerScript;

    public abstract void ActivatePickupEffect();

    public void Awake()
    {
        _playerScript = GameObject.FindWithTag("Player").GetComponent<GameCharacterController2D>();
    }
}