using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [Header("Health Bars (Full → Empty order)")]
    [SerializeField] private GameObject[] healthBars;

    private int currentHealth;

    void Start()
    {
        currentHealth = healthBars.Length;

        // Safety: enable all bars at start
        foreach (var bar in healthBars)
        {
            if (bar != null)
                bar.SetActive(true);
        }
    }

    public void TakeDamage(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            if (currentHealth <= 0)
                return;

            currentHealth--;

            if (healthBars[currentHealth] != null)
                healthBars[currentHealth].SetActive(false);
        }
    }

    public void Heal(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            if (currentHealth >= healthBars.Length)
                return;

            if (healthBars[currentHealth] != null)
                healthBars[currentHealth].SetActive(true);

            currentHealth++;
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
