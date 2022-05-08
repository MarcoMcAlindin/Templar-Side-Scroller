using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Target To Follow")]
    [SerializeField] Transform _target;

    //Components
    private Camera _cameraComponent;
    
    //Offset var
    Vector3 _targetOffset;

    //Initial Postion var
    Vector2 _initialPosition;

    public void OnEnable()
    {
        //Subscribe methods
        GameManager.OnGameRestart += ResetToStartLevel;
    }

    public void OnDisable()
    {
        //Unsubscribe methods
        GameManager.OnGameRestart -= ResetToStartLevel;

    }

    public void Awake()
    {
        //Assign camera component
        _cameraComponent = GetComponent<Camera>();

        //Calculate offset with target
        _targetOffset = new Vector3(transform.position.x - _target.position.x, transform.position.y - _target.position.y, transform.position.z - _target.position.z);

        //Save inital position
        _initialPosition = transform.position;
    }


    public void LateUpdate()
    {
        if (_target != null)
        {
            //Calculate position to follow player in smoothed position
            Vector3 position = _target.position + _targetOffset;

            

            //Check if smoothed position is larger than current position to avoid backwards camera movement
            if (position.x > transform.position.x)
            {
                transform.position = new Vector3(position.x, transform.position.y, position.z);
            }
       

        }
    }

    //Game restart
    void ResetToStartLevel()
    {
        transform.position = _initialPosition;
    }
}
