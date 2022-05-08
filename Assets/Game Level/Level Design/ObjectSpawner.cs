using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public delegate void SpawnEvents();
    public static event SpawnEvents OnSpawnLocationsLoad;

    public static List<ObjectSpawnLocation> _spawnLocations = new List<ObjectSpawnLocation>();

    private void OnEnable()
    {
        ObjectSpawnLocation.OnPlayerNear += SpawnObject;
        GameCharacterController2D.OnPlayerDead += DeactivateAllSpawnLocations;
        GameManager.OnGameRestart += ReactivateAllSpawnLocations;
    }

    private void OnDisable()
    {
        ObjectSpawnLocation.OnPlayerNear -= SpawnObject;
        GameCharacterController2D.OnPlayerDead -= DeactivateAllSpawnLocations;
        GameManager.OnGameRestart -= ReactivateAllSpawnLocations;

    }

    private void Start()
    {
        OnSpawnLocationsLoad?.Invoke();


    }

    public virtual void SpawnObject(ObjectSpawnLocation spawnLocation)
    {
        GameObject tempObj = ObjectPool.Instance.PullObject(spawnLocation.GetObjectName());

        tempObj.transform.position = spawnLocation.Position;

        if (spawnLocation.GetType() == typeof(EnemySpawnLocation))
        {
            EnemyAIController2D enemy = tempObj.GetComponent<EnemyAIController2D>();
            enemy.SetDirection(spawnLocation._direction);
            enemy.SetChaseState(spawnLocation._followPlayer);
            enemy.SetTurnDistance(spawnLocation._followDistance);

        }

    }

    public void DeactivateAllSpawnLocations()
    {
        for (int i = 0; i < _spawnLocations.Count; i++)
        {
            if (_spawnLocations[i].isActiveAndEnabled)
            {
                _spawnLocations[i].gameObject.SetActive(false);
            }
        }
    }

    public void ReactivateAllSpawnLocations()
    {
        for (int i = 0; i < _spawnLocations.Count; i++)
        {

            _spawnLocations[i].gameObject.SetActive(true);

        }
    }
}
