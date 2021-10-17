using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private sceneTypes _gameType;
    [SerializeField] private GameData _data;
    private bool _justOnce = true;
    #endregion

    private void Start()
    {
        switch (_gameType)
        {
            case sceneTypes.SplashScreen:
                StartCoroutine(OpeningRoutine());
                break;
            case sceneTypes.Level:
                break;
        }
    }

    private void OnEnable()
    {
        EventManager.checkingSceneType += ReturnSceneType;
        EventManager.winGame += WinGame;
        EventManager.loseGame += FailGame;
        EventManager.OnStartButton += StartButtonPressed;
    }

    private void OnDisable()
    {
        EventManager.checkingSceneType -= ReturnSceneType;
        EventManager.winGame -= WinGame;
        EventManager.loseGame -= FailGame;
        EventManager.OnStartButton -= StartButtonPressed;
    }

    IEnumerator OpeningRoutine()
    {
        yield return new WaitForSeconds(2f);
        SaveManager.LoadData(_data);
        EventManager.loadOpeningScene?.Invoke();
    }

    private void StartGame()
    {
        EventManager.updateLevelText.Invoke();
        EventManager.startGame.Invoke();
    }

    private void WinGame()
    {
        if (_justOnce)
        {
            EventManager.OnLevelEnd.Invoke();
            SaveManager.SaveData(_data);
            _justOnce = false;
            EventManager.stopWalk.Invoke();
        }
    }

    private void FailGame()
    {
        EventManager.OnTriggerFail.Invoke();
        EventManager.stopWalk.Invoke();
    }

    private bool ReturnSceneType()
    {
        bool loadingScene = false;
        if (_gameType == sceneTypes.SplashScreen)
        {
            loadingScene = true;
        }
        return loadingScene;
    }

    private void StartButtonPressed()
    {
        StartGame();
    }
}