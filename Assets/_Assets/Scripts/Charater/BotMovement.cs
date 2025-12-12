using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMovement : MonoBehaviour, IMovement
{
    [SerializeField] Transform _targetPosition;
    [SerializeField] Transform _targetPosition2;
    private NavMeshAgent _agent;
    private bool _isMoving = false;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        StartMoving(_targetPosition);
        StartCoroutine(WaitTimeSecond());
    }

    private void Update()
    {
        if (!_isMoving) return;
        if(!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            EndMoving();
        }    

    }

    private void StartMoving(Transform targetPostion)
    {
        _isMoving = true;
        _agent.isStopped = false;
        _agent.SetDestination(targetPostion.position);
    }    

    private void EndMoving()
    {
        _isMoving = false;
        _agent.isStopped = true;
    }

    private IEnumerator WaitTimeSecond()
    {
        yield return new WaitForSeconds(5f);
        StartMoving(_targetPosition2);
    }

    // =========== Interface (IMovement) ============
    public bool IsMoving() => _isMoving;

    public void LockInput(bool value) { }

}
