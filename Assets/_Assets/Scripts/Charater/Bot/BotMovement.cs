using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BotMovement : MonoBehaviour, IMovement
{
    private NavMeshAgent _agent;
    private bool _isMoving = false;
    private bool _isCanSelected = false;    

    private Transform _targetTransform;
    private bool _isRotating = false;

    [Header("Rotation setting")]
    [SerializeField] float _rotationSpeed = 20f;
    [SerializeField] float _rotationThreshold = 5f;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!_isMoving || _isCanSelected) return;
        if (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance) return;
        if(_isRotating || _targetTransform == null) return;
        StartCoroutine(RotateToTarget());
    } 

    private IEnumerator RotateToTarget()
    {
        _isRotating = true;
        _agent.isStopped = true;

        Vector3 dir = (_targetTransform.position - transform.position).normalized;
        dir.y = 0;

        if(dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
           
            while(Quaternion.Angle(transform.rotation, targetRotation) > _rotationThreshold)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                yield return null;
            }

        }
        _isRotating = false;
        EndMoving();
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
    public void StartMoving(Transform targetTransform)
    {
        _targetTransform = targetTransform;
        _agent.isStopped = false;
        _agent.SetDestination(targetTransform.position);
        StartCoroutine(DelayArrivalCheck());
    }
}
