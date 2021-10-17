using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    private float _radius = 0.5f;
    public Transform middlePoint;
    private Vector3 _merkez;
    private int _currentNumber;
    public GameObject enemyPefab;
    public int count;
    [SerializeField] private TextMeshPro _currentNumberText;


    private ObjectPool _pool = new ObjectPool();

    private int _poolCount = 20;

    private void Start()
    {
        _currentNumber = 1;
        _merkez = new Vector3(middlePoint.position.x, 1.2447f, middlePoint.position.z);

        FillPool(_pool, enemyPefab, _poolCount);
        var enemy = _pool.Pop();
        enemy.transform.position = _merkez;
        enemy.transform.parent = middlePoint;
        UpdateCount(count);
    }

    private void Update()
    {
        _currentNumberText.text = (this.transform.childCount - 1).ToString();
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


            var enemy = _pool.Pop();
            enemy.transform.position = spawnPos;
            enemy.transform.parent = middlePoint;

        }
    }

    private void UpdateCount(int _number)
    {
        if (_number % 10 != 0)
        {
            int temporyNumber = _number % 10;
            if (temporyNumber > 0)
            {
                _merkez = new Vector3(middlePoint.position.x, 1.2447f, middlePoint.position.z);
                CreateEnemiesAroundPoint(5, _merkez, _radius);
                _radius += 0.05f;
                temporyNumber = temporyNumber - 5;
                CreateEnemiesAroundPoint(temporyNumber, _merkez, _radius);
            }
            _number = _number - temporyNumber;

        }
        int round = _number / 10;
        for (int i = 0; i < round; i++)
        {
            _merkez = new Vector3(middlePoint.position.x, 1.2447f, middlePoint.position.z);
            CreateEnemiesAroundPoint(10, _merkez, _radius);
            _radius += 0.05f;
        }

    }

}
