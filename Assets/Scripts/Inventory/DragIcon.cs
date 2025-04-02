using UnityEngine;
using UnityEngine.UI;
public static class DragIcon
{
    public static GameObject iconObject;
    public static Image iconImage;

    public static void Show(Sprite sprite, Transform parent)
    {
        if (iconObject == null)
        {
            iconObject = new GameObject("DragIcon");
            iconObject.AddComponent<CanvasGroup>().blocksRaycasts = false;

            var img = iconObject.AddComponent<Image>();
            img.raycastTarget = false;
            iconImage = img;

            iconObject.transform.SetParent(parent);
        }

        iconImage.sprite = sprite;
        iconImage.enabled = true;
        iconObject.SetActive(true);
    }

    public static void Move(Vector2 position)
    {
        if (iconObject != null)
        {
            iconObject.transform.position = position;
        }
    }

    public static void Hide()
    {
        if (iconObject != null)
            iconObject.SetActive(false);
    }
}
