using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelObjectType { S_BUSH, L_BUSH, S_WEEDS, L_WEEDS}

public class LevelObjectSpawnLocation : ObjectSpawnLocation
{
    [SerializeField] LevelObjectType _objectType;

    public void Start()
    {
        _activationDistance = 35.0f;

        switch (_objectType)
        {
            case LevelObjectType.S_BUSH:

                _objectName = "Small Bush Prop";
                break;

            case LevelObjectType.L_BUSH:

                _objectName = "Large Bush Prop";
                break;

            case LevelObjectType.S_WEEDS:

                _objectName = "Small Weeds Prop";
                break;

            case LevelObjectType.L_WEEDS:

                _objectName = "Large Weeds Prop";
                break;

            default:

                break;

        }
    }
}
