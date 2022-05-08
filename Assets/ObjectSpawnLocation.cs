using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnLocation : MonoBehaviour
{
    [SerializeField] protected string _objectName;
    public string GetObjectName() { return _objectName; }

    public Vector2 Position { get { return gameObject.transform.position; } }

    [SerializeField] protected float _activationDistance = 10.0f;
    [SerializeField] public Vector2 _direction;
    [SerializeField] public bool _followPlayer;
    [SerializeField] public float _followDistance;


    public delegate void SpawnLocationEvent(ObjectSpawnLocation spawnLocation);
    public static event SpawnLocationEvent OnPlayerNear;


    public void Update()
    {
        CheckDistanceToPlayer();

        Debug.DrawLine(transform.position, GameManager._playerScript.transform.position, Color.green);
    }

    public void OnEnable()
    {
        ObjectSpawner.OnSpawnLocationsLoad += AddLocationToCollection;
    }

    public void OnDisable()
    {
        ObjectSpawner.OnSpawnLocationsLoad -= AddLocationToCollection;

    }

    public void AddLocationToCollection()
    {
        ObjectSpawner._spawnLocations.Add(this);
    }

    public void CheckDistanceToPlayer()
    {
        if (Vector3.Distance(transform.position, GameManager._playerScript.transform.position) < _activationDistance)
        {
            Debug.Log("Object near enough execute event method!");
            OnPlayerNear?.Invoke(this);
            gameObject.SetActive(false);
        }
    }


}
