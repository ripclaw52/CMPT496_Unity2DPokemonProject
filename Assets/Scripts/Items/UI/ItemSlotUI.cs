using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for managing the UI elements of an item slot.
/// </summary>
public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI countText;

    RectTransform rectTransform;

    /// <summary>
    /// Gets the RectTransform component of the current GameObject. 
    /// </summary>
    private void Awake()
    {
        
    }

    public TextMeshProUGUI NameText => nameText;
    public TextMeshProUGUI CountText => countText;
    public float Height => rectTransform.rect.height;

    /// <summary>
    /// Sets the data of the item slot.
    /// </summary>
    /// <param name="itemSlot">The item slot to set the data of.</param>
    public void SetData(ItemSlot itemSlot)
    {
        rectTransform = GetComponent<RectTransform>();
        nameText.text = itemSlot.Item.Name;
        countText.text = $"X {itemSlot.Count}";
    }

    public void SetNameAndPrice(ItemBase item)
    {
        rectTransform = GetComponent<RectTransform>();
        nameText.text = item.Name;
        countText.text = $"$ {item.Price}";
    }
}
