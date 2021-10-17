using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameData _data;
    [SerializeField] private int _levelCount;
    #endregion

    private void OnEnable()
    {
        EventManager.loadOpeningScene += LoadOpeningLevel;
        EventManager.loadNextScene += LoadNextLevel;
        EventManager.loadSameScene += LoadSameLevel;
    }

    private void OnDisable()
    {
        EventManager.loadOpeningScene -= LoadOpeningLevel;
        EventManager.loadNextScene -= LoadNextLevel;
        EventManager.loadSameScene -= LoadSameLevel;
    }

    void LoadOpeningLevel()
    {
        if (_data.levelValue > _levelCount)
        {
            _data.levelValue = 1;
            SaveManager.SaveData(_data);
            SceneManager.LoadScene("level " + _data.levelValue);
        }
        else
        {
            SceneManager.LoadScene("level " + _data.levelValue);
        }
    }

    void LoadNextLevel()
    {
        _data.levelValue++;
        _data.levelTextValue++;
        if (_data.levelValue > _levelCount)
        {
            _data.levelValue = 1;
            SaveManager.SaveData(_data);
            SceneManager.LoadScene("level " + _data.levelValue);
        }
        else
        {
            SaveManager.SaveData(_data);
            SceneManager.LoadScene("level " + _data.levelValue);
        }
    }

    void LoadSameLevel()
    {
        SceneManager.LoadScene("level " + _data.levelValue);
    }
}