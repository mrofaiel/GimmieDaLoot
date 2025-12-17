using UnityEngine;

public class EnemyHealthTest : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 10f;
    [SerializeField] float currentHealth;  // shows in Inspector during Play

    public bool IsDead => currentHealth <= 0f;
    public float CurrentHealth => currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (IsDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage. HP: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject); // DogEnemy disappears
    }
}
