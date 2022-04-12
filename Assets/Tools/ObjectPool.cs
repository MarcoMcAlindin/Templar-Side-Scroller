using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public GameObject[] _gameObjects;

    public List<GameObject>[] _pooledObjects;

    public int[] _amountToBuffer;

    public int _defaultBufferAmount = 3;

    protected GameObject _containerObject;

    private void Start()
    {
        _containerObject = new GameObject("ObjectPool");
        _pooledObjects = new List<GameObject>[_gameObjects.Length];

        int i = 0;

        foreach (GameObject obj in _gameObjects)
        {
            _pooledObjects[i] = new List<GameObject>();

            int bufferAmount;

            if(i < _amountToBuffer[i])
            {
                bufferAmount = _amountToBuffer[i];
            }
            else
            {
                bufferAmount = _defaultBufferAmount;
            }

            for(int n = 0; n < bufferAmount; n++)
            {
                GameObject newObject = Instantiate(obj) as GameObject;

                newObject.name = obj.name;
               
            }

            i++;
        }
    }


    public GameObject PullObject(string objectType)
    {
        bool onlyPooled = false;

        for (int i = 0; i < _gameObjects.Length; i ++)
        {
            GameObject prefab = _gameObjects[i];

            if (prefab.name == objectType)
            {
                if (_pooledObjects[i].Count > 0)
                {
                    
                    GameObject pooledObject = _pooledObjects[i][0];

                    pooledObject.SetActive(true);
                    pooledObject.transform.parent = null;

                    //Remove pooled object
                    _pooledObjects[i].Remove(pooledObject);

                    return pooledObject;
                }
                else if (!onlyPooled)
                {
                    return Instantiate(_gameObjects[i]) as GameObject;
                }

                break;
            }
        }
        return null;
    }

    public void PoolObject(GameObject gameObject)
    {
        for(int i = 0; i < _gameObjects.Length; i++)
        {
            if(_gameObjects[i].name == gameObject.name)
            {
                gameObject.SetActive(false);
                gameObject.transform.parent = _containerObject.transform;
                _pooledObjects[i].Add(gameObject);
                return;
            }
        }

        Destroy(gameObject);
    }

}
