using UnityEngine;
using UnityEngine.SceneManagement;   // loads win scene when 4 

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class LootPickup : MonoBehaviour
{
    // total number of pieces of loot 
    public static int totalLootInLevel = 4;

    // number of pieces of loot collected
    public static int lootCollected = 0;

    [Header("Spin")]
    public float rotateSpeed = 60f;

    [Header("Loot Info")]
    public string lootId = "DefaultLoot";   // setting loot per prefab

    // didnt use
    [Header("Pickup Sound (optional)")]
    public AudioClip pickupSound;

    private AudioSource audioSource;

    private void Reset()
    {
        // collider is trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        // fortnite chest audio
        var a = GetComponent<AudioSource>();
        a.playOnAwake = true;
        a.loop = true;
        a.spatialBlend = 1f;   // 3D
        a.minDistance = 1f;
        a.maxDistance = 15f;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // loot spins
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log($"Loot trigger hit by '{other.name}', root = '{other.transform.root.name}', root tag = {other.transform.root.tag}");

        Transform root = other.transform.root;
        if (!root.CompareTag("Player"))
            return;

        Debug.Log($"Picked up loot: {lootId}");

        // adds loot to inventory
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddLoot(lootId);
        }
        else
        {
            Debug.LogWarning("No InventoryManager.Instance found in scene!");
        }

        // stops the fortnite chest noise
        if (audioSource != null)
        {
            audioSource.Stop();

            if (pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
        }

        // add to total number of loot collected
        lootCollected++;
        Debug.Log($"Loot collected: {lootCollected}/{totalLootInLevel}");

        if (lootCollected >= totalLootInLevel)
        {
            // loads win scene
            SceneManager.LoadScene("WinScene");
        }

        // deletes the object
        Destroy(gameObject, 0.1f);
    }
}


