using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Cloud Stats")]
    [SerializeField] float _cloudSpeed;

    private Transform _playerTransform;

    // Start is called before the first frame update
    void Awake()
    {
        _playerTransform = GameManager._playerScript.transform;

        _cloudSpeed = 2.0f;
    }

    void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            //Reposition clouds when they go out fo the camera for reuse
            if (transform.position.x < _playerTransform.position.x - 65.0f)
            {
                transform.position = new Vector3(_playerTransform.position.x + 60.0f, Random.Range(transform.position.y - 0.5f, transform.position.y + 3.0f), 0.0f);
            }
        }

        //Move clouds 
        transform.Translate(Vector3.left * _cloudSpeed * Time.deltaTime);
    }  
    
}
