using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] FixedJoystick _joystick;
    [SerializeField] float _movementSpeed;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _movementSpeed, _rigidbody.velocity.y, _joystick.Vertical * _movementSpeed);
        if(_joystick.Vertical != 0 || _joystick.Horizontal != 0) transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
    }
    public bool GetInfoMovement()
    {
        if (_joystick.Vertical != 0 || _joystick.Horizontal != 0) return true;
        return false;
    }    

}
