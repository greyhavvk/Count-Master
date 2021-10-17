using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    private float _firstDistance, _currentDistance;

    [Header("Variables")]
    [SerializeField] private Slider _filledBar;
    [SerializeField] private Transform _finishPoint;
    [SerializeField] private Transform _playerPoint;

    private void Start()
    {
        _firstDistance = Mathf.Abs(_finishPoint.position.z - _playerPoint.position.z);
    }

    private void Update()
    {
        OpenProgressBar();
    }

    private void OpenProgressBar()
    {
        if (checkIsFinish())
        {
            _currentDistance = Mathf.Abs(_finishPoint.position.z - _playerPoint.position.z);
        }
        _filledBar.value = (_firstDistance - _currentDistance) / _firstDistance;
    }

    private bool checkIsFinish()
    {
        if (_finishPoint.position.z <= _playerPoint.position.z)
        {
            return false;
        }
        return true;
    }
}