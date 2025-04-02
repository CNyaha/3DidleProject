using UnityEngine;
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public int gold = 1000;

    public Inventory inventory;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        inventory.Initialize(20);
        // UI 초기화
        InventoryUI.Instance.Initialize(inventory);
    }
    public void AddItem(Item item, int count)
    {
        inventory.AddItem(item, count);
        InventoryUI.Instance.Refresh();
    }
}
