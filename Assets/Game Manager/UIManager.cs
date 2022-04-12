using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] LifepointUI[] _lifepointObjects = new LifepointUI[3];

    [SerializeField] GameObject _scoreTextObject;
    [SerializeField] GameObject _playerDialogue;
    [SerializeField] GameObject _soundButton;
    [SerializeField] GameObject _pauseButton;

    private GameCharacterController2D _playerScript;
    private EnemyAIController2D _enemyScript;

    [SerializeField]GameObject _lanndscapeText;

    public GameObject _pauseMenu;

   [SerializeField] public GameObject _gameSpecificUI;
   [SerializeField] public GameObject _gameTitle;

    public void OnEnable()
    {
        GameCharacterController2D.OnCharacterHurt += RemoveLifePointUI;
        HeartPickup.OnHeartPickUp += AddLifepointUI;

        GameManager.OnPause += ShowPauseMenu;
        GameManager.OnUnpause += HidePauseMenu;
        GameManager.OnPause += ShowPlayerDialogue;
        GameManager.OnUnpause += HidePlayerDialogue;
        GameManager.SoundOff += SoundOffUI;
        GameManager.SoundOn += SoundOnUI;
        GameManager.GameStart += StartGameUITransition;

    }

    public void OnDisable()
    {
        GameCharacterController2D.OnCharacterHurt -= RemoveLifePointUI;
        HeartPickup.OnHeartPickUp -= AddLifepointUI;

        GameManager.OnPause -= ShowPauseMenu;
        GameManager.OnUnpause -= HidePauseMenu;
        GameManager.OnPause -= ShowPlayerDialogue;
        GameManager.OnUnpause -= HidePlayerDialogue;
        GameManager.SoundOn -= SoundOnUI;
        GameManager.SoundOff -= SoundOffUI;
        GameManager.GameStart -= StartGameUITransition;



    }

    public void Start()
    {
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<GameCharacterController2D>();

        InitialiseLifepointUI();

    }

    public void UpdateScoreUI(int score)
    {
        Debug.Log(_scoreTextObject);
        
        _scoreTextObject.GetComponent<TMP_Text>().text = score.ToString();
    }

    public void InitialiseLifepointUI()
    {
        Debug.Log("Initialising Lifepoint UI");
        for (int i = 0; i < 3; i++)
        {
            if(i < _playerScript.GetHealthSystem().GetCurrentHealth())
            {
                _lifepointObjects[i].gameObject.SetActive(true);
                Debug.Log("Activating heart" + i + "/" + _playerScript.GetHealthSystem().GetCurrentHealth());

            }
            else
            {
                _lifepointObjects[i].gameObject.SetActive(false);
                Debug.Log("Deactivating heart" + i + "/" + 3);

            }
        }
        
    }

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

    public void RemoveAllLifepointsUI()
    {
        foreach(LifepointUI lp in _lifepointObjects)
        {
            lp.gameObject.SetActive(false);
        }
    }

    public void ShowPauseMenu() { _pauseMenu.SetActive(true); _pauseMenu.GetComponent<Animator>().SetTrigger("Open"); }

    public void HidePauseMenu() { _pauseMenu.GetComponent<Animator>().SetTrigger("Close"); _pauseMenu.SetActive(false); }

    public void ShowPlayerDialogue() { _playerDialogue.SetActive(true); }
    public void HidePlayerDialogue() { _playerDialogue.SetActive(false); }

    public void SoundOnUI() { _soundButton.GetComponent<Image>().color = Color.green; }
    public void SoundOffUI() { _soundButton.GetComponent<Image>().color = Color.red; }

    

    public void LandscapeText(bool b)
    {
        switch (b)
        {
            case false:
                _lanndscapeText.SetActive(false);

                break;
            case true:
                _lanndscapeText.SetActive(true);

                break;
        }

    }

    public void StartGameUITransition()
    {
        _gameSpecificUI.GetComponent<Animator>().SetTrigger("Animate");
        _gameTitle.GetComponent<Animator>().SetTrigger("Animate");
    }

    
}
