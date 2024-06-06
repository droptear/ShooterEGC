using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _mouseSensitivity = 5.0f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _jumpHeight = 2.0f;

    [SerializeField] private Transform _head;
    [SerializeField] private CharacterController _characterController;

    private float _xEuler;
    private Vector3 _velocity;
    private float _runSpeedMultiplier;

    void Update()
    {
        CalculateMovement();
        CalculateRotation();
        SetViewDirection();
        ApplyGravity();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _runSpeedMultiplier = 2.2f;
        }
        else
        {
            _runSpeedMultiplier = 1.0f;
        }

        if (Input.GetKey(KeyCode.LeftControl) || ChechHeight() == false)
        {
            _characterController.height = Mathf.Lerp(_characterController.height, 0.8f, Time.deltaTime * 10.0f);
            _characterController.center = Vector3.Lerp(_characterController.center, new Vector3(0.0f, 0.4f, 0.0f), Time.deltaTime * 10.0f);
            _head.localPosition = Vector3.Lerp(_head.localPosition, new Vector3(0.0f, 0.7f, 0.0f), Time.deltaTime * 10.0f);
        }
        else
        {
            _characterController.height = Mathf.Lerp(_characterController.height, 1.8f, Time.deltaTime * 10.0f);
            _characterController.center = Vector3.Lerp(_characterController.center, new Vector3(0.0f, 0.9f, 0.0f), Time.deltaTime * 10.0f);
            _head.localPosition = Vector3.Lerp(_head.localPosition, new Vector3(0.0f, 1.55f, 0.0f), Time.deltaTime * 10.0f);
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        var moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;
        Vector3 worldVelocity = transform.TransformVector(moveDirection * _speed * _runSpeedMultiplier);
        _velocity = new Vector3(worldVelocity.x, _velocity.y, worldVelocity.z);

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void CalculateRotation()
    {
        float rotationInput = Input.GetAxisRaw("Mouse X");
        var rotation = new Vector3(0.0f, rotationInput, 0.0f);
        transform.Rotate(rotation * _mouseSensitivity);
    }

    private void SetViewDirection()
    {
        _xEuler -= Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
        _xEuler = Mathf.Clamp(_xEuler, -90.0f, 90.0f);

        _head.localEulerAngles = new Vector3(_xEuler, 0.0f, 0.0f);
    }

    private void ApplyGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;

        if (_characterController.isGrounded)
        {
            if(_velocity.y < 0)
            {
                _velocity.y = -1.0f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        _velocity.y += Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
    }

    private bool ChechHeight()
    {
        RaycastHit hitDown;
        RaycastHit hitUp;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        bool isDownHitted = Physics.Raycast(rayOrigin, Vector3.down, out hitDown);
        bool isUpHitted = Physics.Raycast(rayOrigin, Vector3.up, out hitUp); 
        if(isDownHitted && isUpHitted)
        {
            return (hitDown.distance + hitUp.distance > 1.8f);
        }
        else
        {
            return true;
        }
    }
}