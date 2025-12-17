// using UnityEngine;

// public class AnimalSelector : MonoBehaviour
// {
//     [Header("Assign your animal prefabs here (order matters)")]
//     public GameObject[] animalPrefabs;

//     [Header("Where the preview appears in the select scene")]
//     public Transform previewSpawn;

//     private int currentIndex = 0;
//     private GameObject previewInstance;

//     void Start()
//     {
//         Show(currentIndex);
//     }

//     public void Next()
//     {
//         currentIndex = (currentIndex + 1) % animalPrefabs.Length;
//         Show(currentIndex);
//     }

//     public void Prev()
//     {
//         currentIndex = (currentIndex - 1 + animalPrefabs.Length) % animalPrefabs.Length;
//         Show(currentIndex);
//     }

//     public void SelectCurrent()
//     {
//         // Save which prefab to use in the next scene
//         SelectedAnimal.PrefabId = animalPrefabs[currentIndex].name;
//         Debug.Log("Selected: " + SelectedAnimal.PrefabId);
//     }

//     private void Show(int index)
//     {
//         if (previewInstance != null) Destroy(previewInstance);

//         previewInstance = Instantiate(animalPrefabs[index], previewSpawn.position, previewSpawn.rotation);

//         // Optional: stop physics/AI on preview
//         var rb = previewInstance.GetComponent<Rigidbody>();
//         if (rb) rb.isKinematic = true;
//     }
// }
