using System.Collections;
using UnityEngine;

public class ZombieDamage : MonoBehaviour
{
    public int _damage = 2;
    public float knockbackForce = 5f; // Adjust this value as needed for the desired knockback force
    public float knockbackDuration = 0.2f; // Adjust this value for the duration of the knockback
    public HealthManagementSystem playerHealth;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Zombie hits player");
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(_damage);

                // Apply knockback to the player
                StartCoroutine(KnockbackPlayer(other.gameObject.transform));
            }
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
