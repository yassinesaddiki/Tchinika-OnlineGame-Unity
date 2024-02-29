using UnityEngine;

public class CharacterRotation : MonoBehaviour
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
        Aim();
        FlipCharacter();
    }

    private void Aim()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)_weaponObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _weaponObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
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

            // Flip the weapon sprite
            SpriteRenderer weaponSpriteRenderer = _weaponObject.GetComponent<SpriteRenderer>();
            if (weaponSpriteRenderer != null)
            {
                weaponSpriteRenderer.flipY = characterDirection < 0;
            }
        }
    }
}
