using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public Transform spawnPoint;

    void Start()
    {
        int index = PlayerPrefs.GetInt("SelectedAnimalIndex", 0);
        index = Mathf.Clamp(index, 0, animalPrefabs.Length - 1);

        Instantiate(animalPrefabs[index], spawnPoint.position, spawnPoint.rotation);
    }
}
