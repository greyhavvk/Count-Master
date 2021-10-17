using UnityEngine;

public class FinishController : MonoBehaviour
{
#pragma warning disable 0649
    [HideInInspector] public int currentMultiplierZone = 0;

    [Header("Variables")]
    [SerializeField] private FinishFloorNames _finishFloorName;
#pragma warning restore 0649



    private void Start()
    {
        InitValue();

    }

    private void InitValue()
    {
        switch (_finishFloorName)
        {
            case FinishFloorNames.x1:
                currentMultiplierZone = 1;
                break;
            case FinishFloorNames.x2:
                currentMultiplierZone = 2;
                break;
            case FinishFloorNames.x3:
                currentMultiplierZone = 3;
                break;
            case FinishFloorNames.x4:
                currentMultiplierZone = 4;
                break;
            case FinishFloorNames.x5:
                currentMultiplierZone = 5;
                break;
            case FinishFloorNames.x6:
                currentMultiplierZone = 6;
                break;
            case FinishFloorNames.x7:
                currentMultiplierZone = 7;
                break;
            case FinishFloorNames.x8:
                currentMultiplierZone = 8;
                break;
            case FinishFloorNames.x9:
                currentMultiplierZone = 9;
                break;
            case FinishFloorNames.x10:
                currentMultiplierZone = 10;
                break;
            case FinishFloorNames.FinishFlag:
                currentMultiplierZone = 10;
                break;
            default:
                break;
        }
    }
}