using UnityEngine;

public class InventorySortButton : MonoBehaviour
{
    public void OnSortButtonClicked()
    {
        InventoryUI.Instance.SortAndRefresh();
    }
}
