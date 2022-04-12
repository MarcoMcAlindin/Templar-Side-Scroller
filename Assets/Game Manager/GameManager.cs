using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public enum GameState { MAIN_MENU, MAIN_LEVEL }

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public GameState _currentGameState = GameState.MAIN_MENU;

    [SerializeField] public bool _gameOver = false;
    [SerializeField] public bool _isGamePaused = false;
    [SerializeField] public bool _isSoundOn = false;
    [SerializeField] public bool _isGameStarted = false;

    [SerializeField] public static int _playerScore = 0;
    [SerializeField] public static int _prevScore = 0;
    [SerializeField] public static bool _scoreUpdated = true;

    [SerializeField] GameObject _gameUI;


    public delegate void GameActions();
    public static event GameActions OnPause;
    public static event GameActions OnUnpause;
    public static event GameActions SoundOn;
    public static event GameActions SoundOff;
    public static event GameActions GameStart;

    public void Update()
    {
        if (_isGameStarted)
        {
            GameStart?.Invoke();

            _currentGameState = GameState.MAIN_LEVEL;

            _isGameStarted = false;
        }


        if (Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            UIManager.Instance.LandscapeText(false);
            Screen.fullScreen = true;
        }
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight || Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
        {
            UIManager.Instance.LandscapeText(false);
            Screen.fullScreen = true;
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
        

        Debug.Log(_playerScore);
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
            SoundOff?.Invoke();
            _isSoundOn = false;
        }
        else
        {
            SoundOn?.Invoke();
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

}
