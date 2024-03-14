using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectilesOnline : NetworkBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float destroyRadius = 20f;

    //Getters & Setters
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
    private void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
        // Check if the projectile is beyond the destroy radius
    }
    private void DestroyProjectile()
    {
        // Destroy the projectile
        NetworkObject.Destroy(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("I hit " + other.name);
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
        }

        // Destroy the projectile on collision
        Destroy(gameObject);
    }
}
