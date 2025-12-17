using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Slots (HUD icons)")]
    public GameObject[] slots = new GameObject[4];   // drag your 4 HUD objects here

    private int nextFreeSlot = 0;

    private void Awake()
    {
        // Simple singleton so LootPickup can call InventoryManager.Instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Hide all slots at the start
        foreach (var slot in slots)
        {
            if (slot != null)
                slot.SetActive(false);
        }
    }

    public void AddLoot(string lootId)
    {
        if (nextFreeSlot >= slots.Length)
        {
            Debug.Log("Inventory full! Could not add: " + lootId);
            return;
        }

        GameObject slotObj = slots[nextFreeSlot];
        if (slotObj != null)
        {
            slotObj.SetActive(true);
            Debug.Log($"Inventory slot {nextFreeSlot} filled with {lootId}");
        }

        nextFreeSlot++;
    }
}

