using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Respawn (Optional)")]
    public bool respawnOnDeath = false;
    public Transform respawnPoint;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        Debug.Log("Player took damage: " + amount + " | Current HP: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player healed: " + amount + " | Current HP: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("PLAYER DIED!");

        if (respawnOnDeath && respawnPoint != null)
        {
            Respawn();
        }
        else
        {
            // Disable controls or show death UI
            // Example:
            // GetComponent<PlayerMovement>().enabled = false;
            // GetComponent<PlayerLook>().enabled = false;
        }
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        Debug.Log("Player respawned.");
    }
}
