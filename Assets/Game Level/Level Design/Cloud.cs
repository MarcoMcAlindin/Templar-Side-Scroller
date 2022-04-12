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
        _cloudSpeed = 2.0f;
    }

    private void OnEnable()
    {
        if (GameManager.Instance._currentGameState == GameState.MAIN_LEVEL)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }


    // Update is called once per frame
    void LateUpdate()
    {


        if(GameManager.Instance._currentGameState == GameState.MAIN_LEVEL)
        {
            if (_playerTransform != null)
            {

                if (transform.position.x < _playerTransform.position.x - 65.0f)
                {
                    transform.position = new Vector3(_playerTransform.position.x + 55.0f, Random.Range(transform.position.y - 2.5f, transform.position.y + 3.0f), 0.0f);
                }

            }

            
        }
        

        transform.Translate(Vector3.left * _cloudSpeed * Time.fixedDeltaTime);

      
    }
}
