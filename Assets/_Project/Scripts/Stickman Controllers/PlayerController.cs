using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
#pragma warning disable 0649

    [Header("Variables")]
    [SerializeField] private bool _start = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _deltaThreshold;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    private bool _stop = false;
    private bool _finish = false;

    [Header("References")]
    [SerializeField] private Transform _cam;

#pragma warning restore 0649

    private Rigidbody _rigidBodyPlayer;
    private Vector2 _currentTouchPosition;
    private float _finalX;
    private Vector2 _firstPosition;
    private float _currentSpeed;


    private void OnEnable()
    {
        EventManager.startGame += GameStart;
        EventManager.stopWalk += stopWalk;
        EventManager.continueWalk += continueWalk;
        EventManager.stopSlide += stopSlide;
        EventManager.triggerFinishCamera += TriggerFinishCamera;
    }

    private void OnDisable()
    {
        EventManager.startGame -= GameStart;
        EventManager.stopWalk -= stopWalk;
        EventManager.continueWalk -= continueWalk;
        EventManager.stopSlide -= stopSlide;
        EventManager.triggerFinishCamera -= TriggerFinishCamera;
    }
    private void Start()
    {
        _cam.LookAt(this.transform.position);
        AttachReferences();
        ResetValues();
    }

    private void Update()
    {
        StartGame();
    }

    private void FixedUpdate()
    {
        if (_start && !_stop)
        {
            EndlessRun();
        }
    }

    private void StartGame()
    {
        if (_start && !_stop && !_finish)
        {
            MovementWithSlide();
        }
    }


    private void AttachReferences()
    {
        _rigidBodyPlayer = GetComponent<Rigidbody>();
        _currentSpeed = _speed;
    }

    private void ResetValues()
    {
        _rigidBodyPlayer.velocity = new Vector3(0f, _rigidBodyPlayer.velocity.y, _rigidBodyPlayer.velocity.z);
        _firstPosition = Vector2.zero;
        _finalX = 0f;
        _currentTouchPosition = Vector2.zero;
    }

    private void EndlessRun()
    {

        _rigidBodyPlayer.transform.Translate(Vector3.forward * Time.deltaTime * _currentSpeed * 3);
        _cam.parent.transform.Translate(Vector3.forward * Time.deltaTime * _currentSpeed * 3);
    }

    private void MovementWithSlide()
    {
        NavMeshAgent[] navMeshAgents;
        navMeshAgents = this.GetComponentsInChildren<NavMeshAgent>();

        foreach (NavMeshAgent child in navMeshAgents)
        {
            if (child.isOnNavMesh)
                child.SetDestination(transform.position);
        }
        if (Input.GetMouseButtonDown(0))
        {
            _firstPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            _currentTouchPosition = Input.mousePosition;
            Vector2 touchDelta = (_currentTouchPosition - _firstPosition);

            _finalX = transform.position.x;

            if (Mathf.Abs(touchDelta.x) >= _deltaThreshold)
            {
                _finalX = (transform.position.x + touchDelta.x * _sensitivity);
            }

            _rigidBodyPlayer.transform.position = new Vector3(_finalX, transform.position.y, transform.position.z);
            _rigidBodyPlayer.transform.position = new Vector3(Mathf.Clamp(_rigidBodyPlayer.transform.position.x, _minX, _maxX), _rigidBodyPlayer.transform.position.y, _rigidBodyPlayer.transform.position.z);

        }

        if (Input.GetMouseButtonUp(0))
        {
            ResetValues();
        }

    }

    private void GameStart()
    {
        _start = true;
    }
    private void stopWalk()
    {
        _stop = true;
    }
    private void continueWalk()
    {
        _stop = false;
    }
    private void stopSlide()
    {
        _finish = true;
    }

    private void TriggerFinishCamera()
    {
        _cam.parent.transform.position = new Vector3(_cam.parent.transform.position.x + 10, _cam.parent.transform.position.y + 15, _cam.parent.transform.position.z);
        _cam.LookAt(this.transform.position);
    }

}