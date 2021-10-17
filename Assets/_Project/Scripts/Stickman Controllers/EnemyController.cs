using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private bool _fight;
    private Transform[] _allChildren;
    private NavMeshAgent[] _navMeshAgents;

    private void Start()
    {
        _allChildren = this.transform.GetComponentsInChildren<Transform>();
        _fight = false;
    }

    public void Fight(Transform player)
    {
        _navMeshAgents = this.transform.GetComponentsInChildren<NavMeshAgent>();
        foreach (NavMeshAgent child in _navMeshAgents)
        {
            if (child.isOnNavMesh)
                child.SetDestination(player.position);
        }
    }
}
