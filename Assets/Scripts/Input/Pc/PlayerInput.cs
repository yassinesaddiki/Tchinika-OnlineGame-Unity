using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private CharacterRotation _characterRotation;

    private void OnMove(InputValue inputValue)
    {
        Vector2 moveInput = inputValue.Get<Vector2>();
        _characterMovement.SetMoveInput(moveInput);
    }
}
