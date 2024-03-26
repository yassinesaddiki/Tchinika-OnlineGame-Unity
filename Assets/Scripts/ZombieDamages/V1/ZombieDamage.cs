using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public  enum ZombieType
{
    Normal,
    BigBoy
}
public class ZombieDamage : NetworkBehaviour
{
    public int _damage = 2;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public HealthManagementSystem playerHealth;
    [SerializeField] private ZombieType _zombieType;

    private bool isGrabbing = false; // Flag to indicate if BigBoy is grabbing the player

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            switch (_zombieType)
            {
                case ZombieType.Normal:
                    NormalDamage(damageable);
                    break;
                case ZombieType.BigBoy:
                    BigBoyDamage(damageable, other.gameObject);
                    break;
            }
        }
    }

    private void BigBoyDamage(IDamageable damageable, GameObject player)
    {
        if (!isGrabbing)
        {
            isGrabbing = true; // Set grabbing flag to true
            Debug.Log("I Grabbed the player");
            // Stun the player
            // You can implement your own stun mechanism or disable player input temporarily
            StartCoroutine(StunPlayer(player));

            // Apply damage over a period of time
            StartCoroutine(ContinuousDamage(player));

        }
    }

    private IEnumerator ContinuousDamage(GameObject player)
    {
        while (isGrabbing)
        {
            playerHealth.TakeDamage(_damage);
            yield return new WaitForSeconds(1f); // Adjust as needed for damage frequency
        }
    }

    private IEnumerator StunPlayer(GameObject player)
    {
        // Disable player movement
        Debug.Log("I stuned the player");
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();

        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.bodyType = RigidbodyType2D.Static; // Making it static

        //ZombieV1Input _ZombieInput = this.GetComponent<ZombieV1Input>();
        //_ZombieInput.enabled = false;

    // Wait for a certain duration (you can adjust as needed)
    yield return new WaitForSeconds(5f); // Stun duration

        // Enable player movement after stun duration
     
        //_ZombieInput.enabled = true;
        playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        isGrabbing = false;
    }

    private void NormalDamage(IDamageable damageable)
    {
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
            // Apply knockback to the player
            //StartCoroutine(KnockbackPlayer(other.gameObject.transform));
        }
    }

    private IEnumerator KnockbackPlayer(Transform playerTransform)
    {
        Vector2 originalPosition = playerTransform.position;
        Vector2 knockbackDirection = (originalPosition - (Vector2)transform.position).normalized;
        Vector2 targetPosition = originalPosition + knockbackDirection * knockbackForce;

        float elapsedTime = 0f;

        while (elapsedTime < knockbackDuration)
        {
            playerTransform.position = Vector2.Lerp(originalPosition, targetPosition, elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player reaches the target position
        playerTransform.position = targetPosition;
    }
}
