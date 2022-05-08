using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;


public class GameManager : Singleton<GameManager>
{

    //Player Script
    public static GameCharacterController2D _playerScript;

    //Game state booleans
    [SerializeField] public bool _gameOver = false;
    [SerializeField] public bool _isGamePaused = false;
    [SerializeField] public bool _isGameStarted = false;

    //Sound state bool
    [SerializeField] public bool _isSoundOn = false;

    //Player score vars
    [SerializeField] public static int _playerScore = 0;
    [SerializeField] public static int _prevScore = 0;
    [SerializeField] public static bool _scoreUpdated = true;

    //Event handler
    public delegate void GameEvent();

    //Pause Events
    public static event GameEvent OnPause;
    public static event GameEvent OnUnpause;

    //Sound Events
    public static event GameEvent OnSoundOn;
    public static event GameEvent OnSoundOff;

    //Game Start Event
    public static event GameEvent OnGameStart;
    public static event GameEvent OnGameRestart;

    Vector2 _initialPlayerPosition;

    public override void Awake()
    {
        base.Awake();

        _playerScript = GameObject.FindWithTag("Player").GetComponent<GameCharacterController2D>();

        _initialPlayerPosition = _playerScript.transform.position;
    }

    public void Update()
    {
        if (_isGameStarted)
        {
            OnGameStart?.Invoke();
        }


        if (_prevScore <= _playerScore)
        {
            _prevScore += 1;
            UIManager.Instance.UpdateScoreUI(_prevScore);

        }
        else 
        {
            UIManager.Instance.UpdateScoreUI(_playerScore);
            _scoreUpdated = true;
        }
    }

    public IEnumerator EndGame(float delayTime)
    {
        //Wait for delay time in real time seconds
        yield return new WaitForSecondsRealtime(delayTime);
    }

    //Method to be called when game is meant to be paused
    public void PauseGame()
    {
        //Check if paused variable is true or false, pauses or unpauses based on condition
        if (_isGamePaused)
        {
            Time.timeScale = 1.0f;
            _isGamePaused = false;

            OnUnpause?.Invoke();

            if (IsMobilePlatform())
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<GameCharacterController2D>()._touchControlsUI.SetActive(true);
            }
        }
        else
        {
            _isGamePaused = true;

            OnPause?.Invoke();

            if (IsMobilePlatform())
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<GameCharacterController2D>()._touchControlsUI.SetActive(false);
            }
        }

        
    }

    public void GameSoundToggle()
    {
        if (_isSoundOn)
        {
            OnSoundOff?.Invoke();
            _isSoundOn = false;
        }
        else
        {
            OnSoundOn?.Invoke();
            _isSoundOn = true;
        }
    }

    [DllImport("__Internal")]
    private static extern bool IsMobile();

    public bool IsMobilePlatform()
    {
    #if !UNITY_EDITOR
        return IsMobile();
    #endif
        return false;
    }


    public void RestartGame()
    {
        OnGameRestart?.Invoke();
    }
}
