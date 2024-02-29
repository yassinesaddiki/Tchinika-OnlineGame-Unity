using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharRotOnline : NetworkBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private GameObject _weaponObject;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!IsOwner) return;
        Aim();
        FlipCharacter();
    }

    private void Aim()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)_weaponObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the weapon
        _weaponObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Adjust the Y scale based on the rotation angle
        Vector3 weaponScale = _weaponObject.transform.localScale;

        if (angle >= 0f && angle <= 90f)
        {
            // from 0 to 90 degrees, Y Scale is 1
            weaponScale.y = 1f;
        }
        else if (angle > 90f && angle <= 180f)
        {
            // from 90 to 180 degrees, Y Scale is -1
            weaponScale.y = -1f;
        }
        else if (angle >= -180f && angle < -90f)
        {
            // from -180 to -90 degrees, Y Scale is -1
            weaponScale.y = -1f;
        }
        else if (angle > -90f && angle < 0f)
        {
            // from -90 to 0 degrees, Y Scale is 1
            weaponScale.y = 1f;
        }

        _weaponObject.transform.localScale = weaponScale;
    }

    private void FlipCharacter()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float characterDirection = Mathf.Sign(mousePosition.x - transform.position.x);

        if (characterDirection != 0)
        {
            // Flip the body only
            Vector3 newScale = _body.localScale;
            newScale.x = Mathf.Abs(newScale.x) * characterDirection;
            _body.localScale = newScale;
        }
    }
}
