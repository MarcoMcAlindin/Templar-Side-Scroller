using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    GameObject _bkg1, _bkg2, _bkg3;
    GameCharacterController2D _playerscript;

    GameObject _treesObject1, _treesObject2;

    GameObject[] _treeCollection1 = new GameObject[3];
    GameObject[] _treeCollection2 = new GameObject[3];


    float _backgroundWidth;

    //Initial Positions
    Vector2 _bkg1InitialPosition, _bkg2InitialPosition;
    Vector2 _treesObj1InitialPosition, _treesObj2InitialPosition;

    public void OnEnable()
    {
        GameManager.OnGameRestart += ResetAllBackgrounds;

    }

    public void OnDisable()
    {
        GameManager.OnGameRestart -= ResetAllBackgrounds;

    }

    // Start is called before the first frame update
    void Start()
    {
        _playerscript = GameManager._playerScript;

        _bkg1 = transform.GetChild(0).gameObject;
        _bkg2 = transform.GetChild(1).gameObject;
        _bkg3 = transform.GetChild(2).gameObject;


        _backgroundWidth = _bkg2.GetComponent<SpriteRenderer>().bounds.extents.x * 2;

        InitialiseBackgroundTreeObjects();

        GatherInitialBackgroundPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerscript != null)
        {
            if (_playerscript.transform.position.x > _bkg1.transform.position.x)
            {
                _bkg2.transform.position = new Vector3(_bkg1.transform.position.x + _backgroundWidth, 0.0f, 0.0f);

                RandomizeTrees(_treesObject2, _treeCollection2);
            }

            if (_playerscript.transform.position.x > _bkg2.transform.position.x)
            {
                _bkg1.transform.position = new Vector3(_bkg2.transform.position.x + _backgroundWidth, 0.0f, 0.0f);

                RandomizeTrees(_treesObject1, _treeCollection1);

            }
        }        
    }

    void InitialiseBackgroundTreeObjects()
    {
        _treesObject1 = _bkg3.transform.GetChild(0).gameObject;

        for (int i = 0; i < _treeCollection1.Length; i++) { _treeCollection1[i] = _treesObject1.transform.GetChild(i).gameObject; }


        _treesObject2 = _bkg3.transform.GetChild(1).gameObject;

        for (int i = 0; i < _treeCollection2.Length; i++) { _treeCollection2[i] = _treesObject2.transform.GetChild(i).gameObject; }

    }

    void RandomizeTrees(GameObject treeObject ,GameObject[] trees)
    {
        int rand = Random.Range(0,2);

        if(treeObject == _treesObject1)
        {
            _treesObject1.transform.position = new Vector3(_treesObject2.transform.position.x + _backgroundWidth, _treesObject1.transform.position.y, 0.0f);
        }
        else if(treeObject == _treesObject2)
        {
            _treesObject2.transform.position = new Vector3(_treesObject1.transform.position.x + _backgroundWidth, _treesObject2.transform.position.y, 0.0f);
        }

        trees[rand].GetComponent<SpriteRenderer>().flipX = true;
    }

    void GatherInitialBackgroundPositions()
    {
        _bkg1InitialPosition = _bkg1.transform.position;
        _bkg2InitialPosition = _bkg2.transform.position;
        _treesObj1InitialPosition = _treesObject1.transform.position;
        _treesObj2InitialPosition = _treesObject2.transform.position;
    }

    public void ResetAllBackgrounds()
    {
        _bkg1.transform.position = _bkg1InitialPosition;
        _bkg2.transform.position = _bkg2InitialPosition;

        _treesObject1.transform.position = _treesObj1InitialPosition;
        _treesObject2.transform.position = _treesObj2InitialPosition;
    }
}
