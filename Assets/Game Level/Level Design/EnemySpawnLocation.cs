using UnityEngine;

public enum EnemyType { SKELETON, RED_BAT, BLUE_BAT, RED_SKELETON, GOBLIN_BOSS }

public class EnemySpawnLocation : ObjectSpawnLocation
{
    [SerializeField] EnemyType _enemyType;


   

    public void Start()
    {
        _activationDistance = 50.0f;

        

        switch(_enemyType)
        {
            case EnemyType.SKELETON:
                _objectName = "Skeleton Enemy AI";
                break;
            case EnemyType.RED_BAT:
                _objectName = "Red Bat Enemy AI";

                break;
            case EnemyType.BLUE_BAT:
                _objectName = "Blue Bat Enemy AI";

                break;
            case EnemyType.RED_SKELETON:
                _objectName = "Red Skeleton Enemy AI";

                break;
            case EnemyType.GOBLIN_BOSS:
                _objectName = "Goblin Boss Enemy AI";

                break;
            default:
                break;

        }
    }


}
