using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Target To Follow")]
    [SerializeField] Transform _target;

    [SerializeField] float _smoothSpeed = 3.0f;

    //Components
    private Camera _cameraComponent;
    
    Vector3 _targetOffset;

    GameObject _sunBackground;
    GameObject _treeBackground;
    GameObject _trees;



    public void Awake()
    {
        _sunBackground = GameObject.Find("Sun");

        _treeBackground = GameObject.Find("Layer 2");

        _trees = GameObject.Find("Layer 1");


        //Assign camera component
        _cameraComponent = GetComponent<Camera>();

        //Calculate offset with target
        _targetOffset = new Vector3(transform.position.x - _target.position.x, transform.position.y - _target.position.y, transform.position.z - _target.position.z);
    }

    public void FixedUpdate()
    {
        
    }

    public void LateUpdate()
    {
        if (_target != null)
        {
            //Calculate position to follow player in smoothed position
            Vector3 position = _target.position + _targetOffset;

            //Check if smoothed position is larger than current position to avoid backwards camera movement
            if (position.x > transform.position.x && _target.transform.position.x < 955.0f)
            {
                transform.position = new Vector3(position.x, transform.position.y, position.z);

                _sunBackground.transform.Translate(new Vector3(-1.0f, -.5f, 0.0f) * .8f * Time.fixedDeltaTime);

                _treeBackground.transform.Translate(Vector3.left * 2.5f * Time.fixedDeltaTime);

                _trees.transform.Translate(Vector3.left * 8.0f * Time.fixedDeltaTime);
            }
       

        }
    }
}
