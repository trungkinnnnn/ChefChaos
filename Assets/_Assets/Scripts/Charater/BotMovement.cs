using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;

public class BotMovement : MonoBehaviour, IMovement
{
    [SerializeField] Transform _targetPosition;
    [SerializeField] Transform _targetPosition2;
    private NavMeshAgent _agent;
    private bool _isMoving = false;
    private bool _isCanSelected = false;    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        StartMoving(_targetPosition);
        StartCoroutine(WaitTimeSecond());
    }

    private void Update()
    {
        if (!_isMoving || _isCanSelected) return;
        if(!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            EndMoving();
        }    
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
        yield return new WaitForSeconds(6f);
        Debug.Log("Start 2");
        StartMoving(_targetPosition2);
        _isCanSelected = true;
        yield return new WaitForSeconds(0.01f);
        _isCanSelected = false;
    }

    // =========== Interface (IMovement) ============
    public bool IsMoving() => _isMoving;

    public void LockInput(bool value) { }

    // =============== Service ================
    public void StartMoving(Transform targetPostion)
    {
        _isMoving = true;
        _agent.isStopped = false;
        _agent.SetDestination(targetPostion.position);
    }
}
