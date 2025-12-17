using UnityEngine;

public class TimedAnimalSpawner : MonoBehaviour
{
    [Header("What to spawn")]
    public GameObject animalPrefab;

    [Header("Where to spawn")]
    public Transform spawnPoint;

    [Header("Timing")]
    public float spawnEverySeconds = 500f;

    void Start()
    {
        // Spawn immediately, then repeat every X seconds
        InvokeRepeating(nameof(Spawn), 0f, spawnEverySeconds);
    }

    void Spawn()
    {
        if (animalPrefab == null)
        {
            Debug.LogError("[TimedAnimalSpawner] animalPrefab not assigned!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("[TimedAnimalSpawner] spawnPoint not assigned!");
            return;
        }

        Instantiate(animalPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
