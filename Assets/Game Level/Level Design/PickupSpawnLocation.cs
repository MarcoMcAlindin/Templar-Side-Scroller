using UnityEngine;

public enum PickupType { INVINCIBLE, HEART, BULLET }

public class PickupSpawnLocation : ObjectSpawnLocation
{
    [SerializeField] PickupType _pickupType;




    public void Start()
    {
        _activationDistance = 50.0f;



        switch (_pickupType)
        {
            case PickupType.INVINCIBLE:
                _objectName = "Invincible Pickup Prefab";
                break;
            case PickupType.HEART:
                _objectName = "Heart Pickup Prefab";

                break;
            case PickupType.BULLET:
                _objectName = "Bullet Pickup Prefab";

                break;

        }
    }


}
