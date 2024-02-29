using UnityEngine;
using Unity.Netcode;

public class PlayerControl : NetworkBehaviour
{
    [SerializeField]
    private float speed = 5f; // Adjust the speed as needed

    void Update()
    {
        if (!IsOwner) return;

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(xInput, yInput).normalized;
        transform.Translate(speed * Time.deltaTime * moveDirection);
    }
}