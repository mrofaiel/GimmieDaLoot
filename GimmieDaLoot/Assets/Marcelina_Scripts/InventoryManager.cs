using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Slots (HUD icons)")]
    public GameObject[] slots = new GameObject[4];   // creating slot options for the inventory

    private int nextFreeSlot = 0;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // making sure all inventory slots are hidden in the beginning to show empty inventory
        foreach (var slot in slots)
        {
            if (slot != null)
                slot.SetActive(false);
        }
    }

    // adds an item to the next empty slot each time it gets picked up
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

