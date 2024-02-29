using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputOnline : MonoBehaviour
{
    [SerializeField] private CharMoveOnline _characterMovement;

    private void OnMove(InputValue inputValue)
    {
        Vector2 moveInput = inputValue.Get<Vector2>();
        _characterMovement.SetMoveInput(moveInput);
    }
}
