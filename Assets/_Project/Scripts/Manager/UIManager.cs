using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("GameData")]
    [SerializeField] private GameData _data;
    [Header("Panels")]
    [SerializeField] private GameObject _splashPanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _failPanel;
    [SerializeField] private GameObject _tutorialPanel;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _levelText;


    private bool _justOnce = true;
    #endregion

    private void OnEnable()
    {
        EventManager.updateLevelText += UpdateLevelText;
        EventManager.OnLevelEnd += TriggerLevelEndCanvas;
        EventManager.OnTriggerFail += TriggerLevelFailed;
    }

    private void OnDisable()
    {
        EventManager.updateLevelText -= UpdateLevelText;
        EventManager.OnLevelEnd -= TriggerLevelEndCanvas;
        EventManager.OnTriggerFail -= TriggerLevelFailed;
    }

    private void Start()
    {
        ArrangeFirstAppearance();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _justOnce)
        {
            StartButtonPressed();
            _justOnce = false;
        }
    }

    private void ArrangeFirstAppearance()
    {
        UpdateLevelText();
        SaveManager.LoadData(_data);
        CloseAllPanels();
        bool loadingScene = EventManager.checkingSceneType.Invoke();
        if (loadingScene)
        {
            _splashPanel.SetActive(true);
        }
        else if (!loadingScene)
        {
            _gamePanel.SetActive(true);
            _tutorialPanel.SetActive(true);
        }
    }

    private void CloseAllPanels()
    {
        _splashPanel.SetActive(false);
        _gamePanel.SetActive(false);
        _winPanel.SetActive(false);
        _failPanel.SetActive(false);
        _tutorialPanel.SetActive(false);
    }

    private void TriggerLevelEndCanvas()
    {
        _winPanel.SetActive(true);
    }

    private void UpdateLevelText()
    {
        _levelText.text = _data.levelValue.ToString();
    }

    public void TryAgainButtonPressed()
    {
        EventManager.loadSameScene.Invoke();
    }

    public void NextLevelButtonPressed()
    {
        EventManager.loadNextScene.Invoke();

    }

    private void TriggerLevelFailed()
    {
        _failPanel.SetActive(true);
    }

    public void StartButtonPressed()
    {
        EventManager.OnStartButton.Invoke();
        _tutorialPanel.SetActive(false);
    }
}
