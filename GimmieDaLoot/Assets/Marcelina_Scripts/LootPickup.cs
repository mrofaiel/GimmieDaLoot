using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class LootPickup : MonoBehaviour
{
    [Header("Spin")]
    public float rotateSpeed = 60f;

    [Header("Loot Info")]
    public string lootId = "DefaultLoot";   // set per loot prefab (ApplePie, Matcha, etc.)

    [Header("Pickup Sound (optional)")]
    public AudioClip pickupSound;           // sound to play when actually picked up

    private AudioSource audioSource;

    private void Reset()
    {
        // Ensure collider is a trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        // Set up the AudioSource for the idle 3D hum
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
        // Spin around Y axis
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug info so you can see what hits it
        Debug.Log($"Loot trigger hit by '{other.name}', root = '{other.transform.root.name}', root tag = {other.transform.root.tag}");

        // Check tag on the ROOT object (e.g. Kitty_001), not just the child collider
        Transform root = other.transform.root;
        if (!root.CompareTag("Player"))
            return;

        Debug.Log($"Picked up loot: {lootId}");

        // 🔹 1. Add this loot to the inventory
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddLoot(lootId);
        }
        else
        {
            Debug.LogWarning("No InventoryManager.Instance found in scene!");
        }

        // 🔹 2. Stop the idle hum and optionally play a pickup sound
        if (audioSource != null)
        {
            audioSource.Stop();

            if (pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
        }

        // 🔹 3. Remove the pickup from the world
        // (small delay so pickupSound can start)
        Destroy(gameObject, 0.1f);
    }
}


