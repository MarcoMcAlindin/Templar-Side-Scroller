using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : PickupScript
{
    public static event PickUpAction OnHeartPickUp;

    public override void ActivatePickupEffect()
    {
        OnHeartPickUp?.Invoke();
        ObjectPool.Instance.PoolObject(this.gameObject);
    }
}
