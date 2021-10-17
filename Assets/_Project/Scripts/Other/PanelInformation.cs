using UnityEngine;

public class PanelInformation : MonoBehaviour
{
    [SerializeField] private panelType _type;
    [SerializeField] private int _number;


    public int GetNumber()
    {
        return _number;
    }
    public panelType GetType()
    {
        return _type;
    }
}
