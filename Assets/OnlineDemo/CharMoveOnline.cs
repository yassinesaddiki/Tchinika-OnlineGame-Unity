using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharMoveOnline : NetworkBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private Vector2 _smoothMove;
    private Vector2 _inputSmoothMoveVelocity;
    [SerializeField] private float _moveSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(!IsOwner) return;
        SmoothInput();
    }

    private void SmoothInput()
    {
        _smoothMove = Vector2.SmoothDamp(_smoothMove, _moveInput, ref _inputSmoothMoveVelocity, 0.2f);
        _rb.velocity = _smoothMove * _moveSpeed;
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }
}
