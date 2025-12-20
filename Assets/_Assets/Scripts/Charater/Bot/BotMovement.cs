using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;

public class BotMovement : MonoBehaviour, IMovement
{
    private NavMeshAgent _agent;
    private bool _isMoving = false;
    private bool _isCanSelected = false;    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
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

    private IEnumerator DelayArrivalCheck()
    {
        _isMoving = true;
        _isCanSelected = true;
        yield return new WaitForSeconds(0.005f);
        _isCanSelected = false;
    }

    // =========== Interface (IMovement) ============
    public bool IsMoving() => _isMoving;

    public void LockInput(bool value) { }

    // =============== Service ================
    public void StartMoving(Transform targetPostion)
    {
        _agent.isStopped = false;
        _agent.SetDestination(targetPostion.position);
        StartCoroutine(DelayArrivalCheck());
    }
}
