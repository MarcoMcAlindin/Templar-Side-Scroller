using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletPickup : PickupScript
{
    public override void ActivatePickupEffect()
    {

        ObjectPool.Instance.PoolObject(this.gameObject);
    }
}
