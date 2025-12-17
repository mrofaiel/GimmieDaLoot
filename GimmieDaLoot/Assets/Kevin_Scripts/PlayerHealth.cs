using UnityEngine;
using UnityEngine.SceneManagement;   // <-- required for scene loading

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
        Debug.Log("PLAYER DIED! Loading LoseScene...");

        if (respawnOnDeath && respawnPoint != null)
        {
            Respawn();
        }
        else
        {
            // Load the Lose Scene
            SceneManager.LoadScene("LoseScene");
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
