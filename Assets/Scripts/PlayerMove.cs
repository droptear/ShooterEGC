using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _mouseSesitivity = 5.0f;
    [SerializeField] private Transform _head;
    [SerializeField] private CharacterController _characterController;

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        var moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;
        Vector3 localVelocity = moveDirection * _speed;
        Vector3 worldVelocity = transform.TransformVector(localVelocity);

        _characterController.Move(worldVelocity * Time.deltaTime);
    }
}
