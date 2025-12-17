using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LootPickup : MonoBehaviour
{
    [Header("Spin")]
    public float rotateSpeed = 60f;

    [Header("Loot Info")]
    public string lootId = "DefaultLoot";   // can change per instance later

    private void Reset()
    {
        // Ensure collider is trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Update()
    {
        // Spin around Y axis
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Always log who touched us so we can debug
        Debug.Log($"Loot trigger hit by '{other.name}', root = '{other.transform.root.name}', root tag = {other.transform.root.tag}");

        // Check tag on the ROOT object (e.g. Kitty_001), not just the child collider
        Transform root = other.transform.root;
        if (!root.CompareTag("Player"))
            return;

        Debug.Log($"Picked up loot: {lootId}");

        // TODO later: InventoryManager.Instance.AddLoot(lootId);

        gameObject.SetActive(false);   // hide pickup
    }
}

