using System;

public static class EventManager
{
    public static Action stopSlide;

    #region StickmanController
    public static Action StopAndFight;
    public static Action<int> IncreaseNumberWithAdd;
    public static Action<int> IncreaseNumberWithMult;
    public static Action DecreaseNumber;
    public static Action stopWalk;
    public static Action continueWalk;
    public static Action startRunAnim;
    public static Action triggerFinish;

    #endregion

    #region UIManagerEvents
    public static Action OnStartButton;
    public static Action OnLevelEnd;
    public static Action OnTriggerFail;
    public static Action updateLevelText;
    #endregion

    #region SpawnerEvents
    public static Action triggerFinishCamera;
    #endregion

    #region GameManagerEvents
    public static Action startGame;
    public static Action winGame;
    public static Action loseGame;
    #endregion

    #region LevelManagerEvents
    public static Action loadNextScene;
    public static Action loadOpeningScene;
    public static Action loadSameScene;
    #endregion

    #region UserInterfaceEvents
    public static Func<bool> checkingSceneType;
    #endregion

    public static ObjectPool playerPool = new ObjectPool();
}