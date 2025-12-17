using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class LootPickup : MonoBehaviour
{
    [Header("Spin")]
    public float rotateSpeed = 60f;

    [Header("Loot Info")]
    public string lootId = "DefaultLoot";   // can change per instance later

    private AudioSource audioSource;

    private void Reset()
    {
        // Ensure collider is trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        // Make sure the AudioSource is set up for 3D loot hum
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

        // Stop the humming sound
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // TODO later: InventoryManager.Instance.AddLoot(lootId);

        // Hide / disable the pickup
        gameObject.SetActive(false);
    }
}

