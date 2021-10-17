using UnityEngine;
using UnityEngine.AI;

public class StickmanController : MonoBehaviour
{

    private bool _fight;
    private Transform _stickman;
    private Transform _enemy;
    private bool _justOne = true;
    private bool _justOneAgain = true;

    private void OnEnable()
    {
        EventManager.startRunAnim += GameStart;
    }

    private void OnDisable()
    {
        EventManager.startRunAnim -= GameStart;
    }
    private void Start()
    {
        _stickman = this.transform;
        _fight = false;
    }

    private void Update()
    {

    }

    private void Fight()
    {

        NavMeshAgent[] navMeshAgents;
        navMeshAgents = this.transform.parent.GetComponentsInChildren<NavMeshAgent>();

        foreach (NavMeshAgent child in navMeshAgents)
        {
            if (child.isOnNavMesh)
                child.SetDestination(_stickman.position);
        }
        EventManager.stopWalk.Invoke();
    }

    public void KillEachOther(Collider other)
    {

        other.enabled = false;
        EventManager.DecreaseNumber.Invoke();

        if (_stickman.transform.parent.childCount <= 2)
        {

        }

        if (other.transform.parent.childCount <= 2)
        {
            other.transform.parent.gameObject.SetActive(false);
            EventManager.continueWalk.Invoke();
        }
        Destroy(other.gameObject);
        //var _instance = Object.Instantiate(deadVfx, stickman.position, Quaternion.identity) as GameObject;
        //var _enemyInstance = Object.Instantiate(enemyDeadVfx, other.transform.position, Quaternion.identity) as GameObject;
        _stickman.transform.parent = null;
        EventManager.playerPool.Push(_stickman.gameObject);
        //Destroy(stickman.gameObject);

    }

    private void Dead()
    {
        EventManager.DecreaseNumber.Invoke();
        //var _instance = Object.Instantiate(deadVfx, stickman.position, Quaternion.identity) as GameObject;
        _stickman.transform.parent = null;
        EventManager.playerPool.Push(_stickman.gameObject);
        //Destroy(stickman.gameObject);
    }

    private void GameStart()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            if (_justOne)
            {
                KillEachOther(other);
                _justOne = false;
            }
        }
        if (other.CompareTag("fight"))
        {
            if (_justOneAgain)
            {
                Fight();
                other.GetComponent<EnemyController>().Fight(_stickman);
                _justOneAgain = false;
            }
        }
        else if (other.CompareTag("barrier"))
        {
            if (_justOne)
            {
                Dead();
                _justOne = false;
            }

        }
        else if (other.CompareTag("panel"))
        {
            if (other.GetComponent<PanelInformation>().GetType() == panelType.Mult)
            {
                EventManager.IncreaseNumberWithMult.Invoke(other.GetComponent<PanelInformation>().GetNumber());
            }
            else if (other.GetComponent<PanelInformation>().GetType() == panelType.Add)
            {
                EventManager.IncreaseNumberWithAdd.Invoke(other.GetComponent<PanelInformation>().GetNumber());
            }

            other.transform.parent.parent.gameObject.SetActive(false);
        }
        else if (other.CompareTag("finish"))
        {
            EventManager.triggerFinish.Invoke();
        }
        if (other.CompareTag("navmeshOffTrigger"))
        {
            this.GetComponent<NavMeshAgent>().enabled = false;
            this.GetComponent<Rigidbody>().drag = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("navmeshOffTrigger"))
        {
            this.GetComponent<NavMeshAgent>().enabled = true;
            this.GetComponent<Rigidbody>().drag = 30;
        }
    }
}
