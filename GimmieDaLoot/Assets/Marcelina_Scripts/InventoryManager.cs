using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [System.Serializable]
    public class InventorySlot
    {
        public string lootId;      // must match LootPickup.lootId
        public GameObject slotObj; // the HUD icon GameObject
        [HideInInspector] public bool filled;
    }

    [Header("Inventory Slots (one per item)")]
    public InventorySlot[] slots;

    [Header("Win Settings")]
    public int requiredLootCount = 4;      // number of unique items to win
    public string winSceneName = "WinScene";

    private int uniqueCollected = 0;

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
        // hide all slot icons at start
        foreach (var s in slots)
        {
            if (s.slotObj != null)
                s.slotObj.SetActive(false);

            s.filled = false;
        }

        uniqueCollected = 0;
    }

    public void AddLoot(string lootId)
    {
        // find the slot configured for this lootId
        for (int i = 0; i < slots.Length; i++)
        {
            var s = slots[i];

            if (s.lootId == lootId)
            {
                if (s.filled)
                {
                    Debug.Log($"Loot '{lootId}' already collected; ignoring.");
                    return;
                }

                s.filled = true;
                uniqueCollected++;

                if (s.slotObj != null)
                    s.slotObj.SetActive(true);

                Debug.Log($"Activated slot {i} for '{lootId}'. Unique collected: {uniqueCollected}/{requiredLootCount}");

                if (uniqueCollected >= requiredLootCount)
                {
                    Debug.Log("All required loot collected, loading win scene.");
                    SceneManager.LoadScene(winSceneName);
                }

                return;
            }
        }

        Debug.LogWarning($"No inventory slot configured for lootId '{lootId}'.");
    }
}
