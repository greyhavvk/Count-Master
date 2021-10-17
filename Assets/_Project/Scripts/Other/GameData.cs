using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/Game Data")]
public class GameData : ScriptableObject
{
    public int levelValue;
    public int levelTextValue;
    public int tutorialLevelCount;
    public int currentScore;
}
