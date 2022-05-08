using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] LifepointUI[] _lifepointObjects = new LifepointUI[3];

    [SerializeField] GameObject _scoreTextObject;
    [SerializeField] GameObject _soundButton;
    [SerializeField] GameObject _pauseButton;

    private GameCharacterController2D _playerScript;
    private EnemyAIController2D _enemyScript;

    [SerializeField]GameObject _lanndscapeText;

    public GameObject _pauseMenu;
    [SerializeField] GameObject _gameOverMenu;


   [SerializeField] public GameObject _gameSpecificUI;
   [SerializeField] public GameObject _gameTitle;

    public static readonly string ANIM_UI = "AnimateUI";

    public void OnEnable()
    {
        //---Subscribe class methods to events---

        //Player Related Events
        GameCharacterController2D.OnCharacterHurt += RemoveLifePointUI;
        GameCharacterController2D.OnPlayerDead += ShowGameOverMenu;

        //Pick Up Events
        HeartPickup.OnHeartPickUp += AddLifepointUI;

        //Pause Events
        GameManager.OnPause += ShowPauseMenu;
        GameManager.OnUnpause += HidePauseMenu;
       

        //Sound Events
        GameManager.OnSoundOff += SoundOffUI;
        GameManager.OnSoundOn += SoundOnUI;

        //Game start Events
        GameManager.OnGameStart += StartGameUITransition;
        GameManager.OnGameRestart += HideGameOverMenu;
        GameManager.OnGameRestart += InitialiseLifepointUI;




    }

    public void OnDisable()
    {
        //---Unsubsribe to Events

        //Player Related Events
        GameCharacterController2D.OnCharacterHurt -= RemoveLifePointUI;
        GameCharacterController2D.OnPlayerDead -= ShowGameOverMenu;

        //Pick Up Events
        HeartPickup.OnHeartPickUp -= AddLifepointUI;

        //Pause Events
        GameManager.OnPause -= ShowPauseMenu;
        GameManager.OnUnpause -= HidePauseMenu;
      

        //Sound Events
        GameManager.OnSoundOn -= SoundOnUI;
        GameManager.OnSoundOff -= SoundOffUI;

        //Game Start Events
        GameManager.OnGameStart -= StartGameUITransition;
        GameManager.OnGameRestart -= HideGameOverMenu;
        GameManager.OnGameRestart -= InitialiseLifepointUI;

    }

    public void Start()
    {
        _playerScript = GameManager._playerScript;

        InitialiseLifepointUI();

    }

    /**********************
            Score UI
     **********************/

    public void UpdateScoreUI(int score)
    {        
        _scoreTextObject.GetComponent<TMP_Text>().text = score.ToString();
    }

    //----------------------------

    /*****************************************
               Health System UI
    /*****************************************/

    //Initialise health system UI for player 
    public void InitialiseLifepointUI()
    {
        for (int i = 0; i < 3; i++)
        {
            if(i < _playerScript.GetHealthSystem().GetCurrentHealth())
            {
                _lifepointObjects[i].gameObject.SetActive(true);
            }
            else
            {
                _lifepointObjects[i].gameObject.SetActive(false);
            }
        }
        
    }

    //Remove Life heart
    public void RemoveLifePointUI()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!_lifepointObjects[i].gameObject.activeSelf && i != 0)
            {
                _lifepointObjects[i - 1].gameObject.SetActive(false);
            }
            else
            {
                _lifepointObjects[_lifepointObjects.Length - 1].gameObject.SetActive(false);
            }
        }
    }

    //Add heart
    public void AddLifepointUI()
    {
         for (int i = 0; i < 3; i++)
         {
            if (!_lifepointObjects[i].gameObject.activeSelf)
            {
                _lifepointObjects[i].gameObject.SetActive(true);
                return;
            }
         }
    }

    //Remove all lifepoints (used when player fall off platforms and dies)
    public void RemoveAllLifepointsUI()
    {
        foreach(LifepointUI lp in _lifepointObjects)
        {
            lp.gameObject.SetActive(false);
        }
    }

    //---------------------------------------


    /*********************************
            Pause Menu UI
     *********************************/
    public void ShowPauseMenu()
    {
        _pauseMenu.SetActive(true);

        _pauseMenu.GetComponent<Animator>().SetBool(ANIM_UI, true);
    }

    public void HidePauseMenu()
    { 
        _pauseMenu.SetActive(false);
        _pauseMenu.GetComponent<Animator>().SetBool(ANIM_UI, false);

    }

    //----------------------------------

    /*******************************************
     *      Game Over Menu
     ******************************************/

    public void ShowGameOverMenu()
    {
        _gameOverMenu.SetActive(true);
        _gameOverMenu.GetComponent<Animator>().SetBool("AnimateUI", true);

    }


    public void HideGameOverMenu()
    {
        _gameOverMenu.SetActive(false);
        _gameOverMenu.GetComponent<Animator>().SetBool("AnimateUI", false);

    }

    /***********************************
              Sound System UI
    *************************************/
    public void SoundOnUI() { _soundButton.GetComponent<Image>().color = Color.green; }
    public void SoundOffUI() { _soundButton.GetComponent<Image>().color = Color.red; }


    public void StartGameUITransition()
    {
        _gameSpecificUI.GetComponent<Animator>().SetTrigger(ANIM_UI);
        _gameTitle.GetComponent<Animator>().SetTrigger(ANIM_UI);

    }

    
}
