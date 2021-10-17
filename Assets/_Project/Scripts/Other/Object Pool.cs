using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject _prefab;
    private Stack<GameObject> _objectPool = new Stack<GameObject>();

    public void SetObject(GameObject prefab)
    {
        this._prefab = prefab;
    }

    public int GetLength()
    {
        return _objectPool.Count;
    }

    public void Fill(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Object.Instantiate(_prefab) as GameObject;
            obj.gameObject.SetActive(false);

            Push(obj);
        }
    }

    public GameObject Pop()
    {
        GameObject obj;
        if (_objectPool.Count > 0)
            obj = _objectPool.Pop();
        else
            obj = Object.Instantiate(_prefab) as GameObject;

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Push(GameObject obj)
    {
        if (obj != null)
        {
            _objectPool.Push(obj);
            obj.gameObject.SetActive(false);
            if (obj.transform.parent != null)
            {
                obj.transform.parent = null;
            }
        }

    }

    public bool IsObjectNull()
    {
        return _prefab == null;
    }
}