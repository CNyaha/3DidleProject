using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;
    public InventoryUI inventoryUI;
    public int slotCount = 20;
    private void Awake()
    {
        if (inventory == null)
            inventory = GetComponent<Inventory>();

        if (inventoryUI == null)
            inventoryUI = FindObjectOfType<InventoryUI>();
    }
    private void Start()
    {
        inventory.Initialize(slotCount);
        inventoryUI.Initialize(inventory);
    }
}
