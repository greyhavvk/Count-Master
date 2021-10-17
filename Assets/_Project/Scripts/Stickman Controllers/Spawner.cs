using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Spawner : MonoBehaviour
{
    private float _radius = 0.5f;
    [SerializeField] private Transform _middlePoint;
    private Vector3 _merkez;
    private int _currentNumber = 1;
    [SerializeField] private GameObject _playerPrefab;
    private int _count = 1;
    private int _kat = 1;
    private int _arakat;
    private float _high;
    private bool _stop = false;
    private bool _finish = false;
    private Vector3 _point;
    [SerializeField] private TextMeshPro _currentNumberText;

    private int _poolCount = 20;

    private void OnEnable()
    {
        EventManager.IncreaseNumberWithAdd += Addition;
        EventManager.IncreaseNumberWithMult += Multiplication;
        EventManager.DecreaseNumber += DecreaseNumber;
        EventManager.triggerFinish += FinishAnimation;
    }

    private void OnDisable()
    {
        EventManager.IncreaseNumberWithAdd -= Addition;
        EventManager.IncreaseNumberWithMult -= Multiplication;
        EventManager.DecreaseNumber -= DecreaseNumber;
        EventManager.triggerFinish -= FinishAnimation;
    }
    private void Start()
    {
        FillPool(EventManager.playerPool, _playerPrefab, _poolCount);
        _merkez = new Vector3(_middlePoint.position.x, 1.2447f, _middlePoint.position.z);
        UpdateCount(_currentNumber);
        _currentNumberText.text = _currentNumber.ToString();
    }

    private void FillPool(ObjectPool pool, GameObject prefab, int count)
    {
        pool.SetObject(prefab);
        pool.Fill(count);
    }

    private void CreateEnemiesAroundPoint(int num, Vector3 point, float radius)
    {

        for (int i = 0; i < num; i++)
        {
            var radians = 2 * Mathf.PI / num * i;

            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            var spawnPos = point + spawnDir * radius;

            //var _object = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            var _object = EventManager.playerPool.Pop();
            _object.transform.position = spawnPos;

            _object.transform.parent = _middlePoint;
            if (_object.GetComponent<NavMeshAgent>().isOnNavMesh)
                _object.GetComponent<NavMeshAgent>().SetDestination(_middlePoint.position);

        }
    }

    private void Addition(int add)
    {
        _currentNumber += add;
        UpdateCount(_currentNumber);
    }

    private void Multiplication(int mult)
    {
        _currentNumber = _currentNumber * mult;
        UpdateCount(_currentNumber);
    }

    private void UpdateCount(int _number)
    {
        Debug.Log(_number);
        StickmanController[] _transform;
        _transform = this.GetComponentsInChildren<StickmanController>();

        foreach (StickmanController child in _transform)
        {
            EventManager.playerPool.Push(child.gameObject);
            //Destroy(child.gameObject);
            child.transform.parent = null;
        }
        if (_number % 10 != 0)
        {
            int temporyNumber = _number % 10;
            if (temporyNumber > 0)
            {
                _merkez = new Vector3(_middlePoint.position.x, 1.2447f, _middlePoint.position.z);
                _radius += 0.05f;
                CreateEnemiesAroundPoint(temporyNumber, _merkez, _radius);
            }
            _number = _number - temporyNumber;

        }
        int round = _number / 10;
        for (int i = 0; i < round; i++)
        {
            _merkez = new Vector3(_middlePoint.position.x, 1.2447f, _middlePoint.position.z);
            CreateEnemiesAroundPoint(10, _merkez, _radius);
            _radius += 0.05f;
        }
        _currentNumberText.text = _currentNumber.ToString();
    }

    private IEnumerator GoAndStop()
    {

        NavMeshAgent[] navMeshAgents;
        navMeshAgents = this.transform.GetComponentsInChildren<NavMeshAgent>();

        foreach (NavMeshAgent child in navMeshAgents)
        {
            child.isStopped = false;
            child.SetDestination(_middlePoint.position);
        }
        yield return new WaitForSeconds(0.5f);

        navMeshAgents = this.transform.GetComponentsInChildren<NavMeshAgent>();

        foreach (NavMeshAgent child in navMeshAgents)
        {
            child.isStopped = true;
        }
    }

    private void DecreaseNumber()
    {
        _currentNumber--;
        _currentNumberText.text = _currentNumber.ToString();
        if (_currentNumber <= 0)
        {
            EventManager.loseGame.Invoke();

        }
    }

    private void FinishAnimation()
    {
        EventManager.stopSlide.Invoke();
        _point = new Vector3(_middlePoint.position.x, _middlePoint.position.y, _middlePoint.position.z);
        StickmanController[] _transform;
        _transform = this.GetComponentsInChildren<StickmanController>();

        foreach (StickmanController child in _transform)
        {
            EventManager.playerPool.Push(child.gameObject);
            //Destroy(child.gameObject);
        }
        Destroy(this.gameObject.transform.GetChild(0).gameObject);

        while (_count < _currentNumber)
        {
            _kat++;
            _count += _kat;
        }

        if ((_count - _currentNumber) > 0)
        {
            _arakat = _count - _currentNumber;
            _kat--;
        }

        StartCoroutine(StartFinish());

    }
    private IEnumerator StartFinish()
    {
        EventManager.triggerFinishCamera.Invoke();
        for (int i = _kat; i > 0; i--)
        {
            if (_arakat == i)
            {
                Diz(_arakat);
                yield return new WaitForSeconds(0.3f);
            }
            Diz(i);
            yield return new WaitForSeconds(0.3f);
        }
        _kat++;
    }

    private IEnumerator CloseGravity(GameObject _object)
    {
        yield return new WaitForSeconds(0.3f);
        _object.GetComponent<Rigidbody>().useGravity = false;
    }
    private void Diz(int i)
    {
        if ((i % 2) == 0 && (i > 0))
        {
            float k = ((-1) * 0.65f * ((i / 2) - 1)) - 0.325f;
            for (int x = 0; x < i; x++)
            {
                _point = new Vector3(k, _point.y, _middlePoint.position.z);
                var stickman = EventManager.playerPool.Pop();  //Instantiate(finishPrefab, _point, Quaternion.identity) as GameObject;
                stickman.transform.position = _point;
                stickman.transform.rotation = Quaternion.identity;
                Destroy(stickman.GetComponent<StickmanController>());
                Destroy(stickman.GetComponent<NavMeshAgent>());
                stickman.GetComponent<Rigidbody>().drag = 0;
                stickman.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                stickman.AddComponent(typeof(FinishStickmanController));
                stickman.transform.parent = _middlePoint;
                k += 0.65f;
            }
        }
        else if ((i % 2) != 0 && (i > 0))
        {
            float k = (-1) * 0.65f * (i / 2);
            for (int x = 0; x < i; x++)
            {
                _point = new Vector3(k, _point.y, _middlePoint.position.z);
                var stickman = EventManager.playerPool.Pop();  //Instantiate(finishPrefab, _point, Quaternion.identity) as GameObject;
                stickman.transform.position = _point;
                stickman.transform.rotation = Quaternion.identity;
                Destroy(stickman.GetComponent<StickmanController>());
                Destroy(stickman.GetComponent<NavMeshAgent>());
                stickman.GetComponent<Rigidbody>().drag = 0;
                stickman.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                stickman.AddComponent(typeof(FinishStickmanController));
                stickman.transform.parent = _middlePoint;
                k += 0.65f;
            }
        }

        _point = new Vector3(0, _point.y + 1.85f, _middlePoint.position.z);
    }
}
