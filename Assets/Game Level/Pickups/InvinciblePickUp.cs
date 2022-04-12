using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePickUp : PickupScript
{
    public static event PickUpAction OnInvinciblePickUp;


    public override void ActivatePickupEffect()
    {
        //Invoke event
        OnInvinciblePickUp?.Invoke();

        //Send pick up object to pool
        ObjectPool.Instance.PoolObject(this.gameObject);
    }
}