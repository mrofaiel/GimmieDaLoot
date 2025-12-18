using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class LootPickup : MonoBehaviour
{
    [Header("Spin")]
    public float rotateSpeed = 60f;

    [Header("Loot Info")]
    public string lootId = "DefaultLoot";   // set per prefab in Inspector

    [Header("Pickup Sound (optional)")]
    public AudioClip pickupSound;

    private AudioSource audioSource;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        var a = GetComponent<AudioSource>();
        a.playOnAwake = true;
        a.loop = true;
        a.spatialBlend = 1f;
        a.minDistance = 1f;
        a.maxDistance = 15f;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Loot trigger hit by '{other.name}', root = '{other.transform.root.name}', root tag = {other.transform.root.tag}");

        Transform root = other.transform.root;
        if (!root.CompareTag("Player"))
            return;

        Debug.Log($"Picked up loot: {lootId}");

        // Tell the inventory
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddLoot(lootId);
        }
        else
        {
            Debug.LogWarning("No InventoryManager.Instance found in scene!");
        }

        // stop ambient fortnite chest sound
        if (audioSource != null)
        {
            audioSource.Stop();

            if (pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
        }

        // destroy after
        float delay = (pickupSound != null) ? pickupSound.length : 0.1f;
        Destroy(gameObject, delay);
    }
}



