using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovement
{
    [SerializeField] FixedJoystick _joystick;
    [SerializeField] float _movementSpeed;

    private bool _lockInput = false;    
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_lockInput)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }    

        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _movementSpeed, _rigidbody.velocity.y, _joystick.Vertical * _movementSpeed);
        if(_joystick.Vertical != 0 || _joystick.Horizontal != 0) transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
    }


    // ============= Interface (IMovement) ================
    public bool IsMoving() => _joystick.Vertical != 0 || _joystick.Horizontal != 0;      

    public void LockInput(bool value) => _lockInput = value;   

}
