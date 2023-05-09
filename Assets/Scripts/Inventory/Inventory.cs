using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Enum representing the different categories of items in the game.
/// </summary>
//public enum ItemCategory { Items, Pokeballs, Tms }
public enum ItemCategory { Medicines, Pokeballs, BattleItems, Berries, OtherItems, Tms, Treasures, KeyItems }

/// <summary>
/// This class is used to manage the inventory of the game.
/// </summary>
public class Inventory : MonoBehaviour
{
    //
    [SerializeField] List<ItemSlot> medicineSlots;
    [SerializeField] List<ItemSlot> pokeballSlots;
    [SerializeField] List<ItemSlot> battleItemSlots;
    [SerializeField] List<ItemSlot> berrySlots;
    [SerializeField] List<ItemSlot> otherItemSlots;
    [SerializeField] List<ItemSlot> tmSlots;
    [SerializeField] List<ItemSlot> treasureSlots;
    [SerializeField] List<ItemSlot> keyItemSlots;
    //
    /*/
    [SerializeField] List<ItemSlot> slots;
    [SerializeField] List<ItemSlot> pokeballSlots;
    [SerializeField] List<ItemSlot> tmSlots;
    /*/

    List<List<ItemSlot>> allSlots;

    public event Action OnUpdated;

    /// <summary>
    /// Creates a list of all item slots, including medicine, pokeball, battle item, berry, other item, TM, treasure, and key item slots.
    /// </summary>
    private void Awake()
    {
        //allSlots = new List<List<ItemSlot>>() { slots, pokeballSlots, tmSlots };
        allSlots = new List<List<ItemSlot>>() { medicineSlots, pokeballSlots, battleItemSlots, berrySlots, otherItemSlots, tmSlots, treasureSlots, keyItemSlots };
    }

    /// <summary>
    /// Gets or sets a list of item categories.
    /// </summary>
    /// <returns>A list of item categories.</returns>
    public static List<string> ItemCategories { get; set; } = new List<string>()
    {
        //"ITEMS", "POKEBALLS", "TMs & HMs"
        "Medicines", "Poke Balls", "Battle Items", "Berries", "Other Items", "TMs & HMs", "Treasures", "Key Items"
    };

    /// <summary>
    /// Gets a list of ItemSlots for a given category index.
    /// </summary>
    /// <param name="categoryIndex">The index of the category to get slots for.</param>
    /// <returns>A list of ItemSlots for the given category index.</returns>
    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        return allSlots[categoryIndex];
    }

    /// <summary>
    /// Retrieves an item from a given category index and item index.
    /// </summary>
    /// <param name="itemIndex">The index of the item to retrieve.</param>
    /// <param name="categoryIndex">The index of the category to retrieve from.</param>
    /// <returns>The item at the given index.</returns>
    public ItemBase GetItem(int itemIndex, int categoryIndex)
    {
        var currentSlots = GetSlotsByCategory(categoryIndex);
        return currentSlots[itemIndex].Item;
    }

    /// <summary>
    /// Uses the item with the given index on the selected Pokemon.
    /// </summary>
    /// <param name="itemIndex">The index of the item to use.</param>
    /// <param name="selectedPokemon">The Pokemon to use the item on.</param>
    /// <param name="selectedCategory">The category of the item.</param>
    /// <returns>The item used, or null if the item could not be used.</returns>
    public ItemBase UseItem(int itemIndex, Pokemon selectedPokemon, int selectedCategory)
    {
        var item = GetItem(itemIndex, selectedCategory);
        bool itemUsed = item.Use(selectedPokemon);
        if (itemUsed)
        {
            if (!item.IsResuable)
                RemoveItem(item, selectedCategory);
            return item;
        }

        return null;
    }

    /// <summary>
    /// Adds an item to the inventory, or increases the count of an existing item.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="count">The number of items to add. Defaults to 1.</param>
    public void AddItem(ItemBase item, int count = 1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.FirstOrDefault(slot => slot.Item == item);
        if (itemSlot != null)
        {
            itemSlot.Count += count;
        }
        else
        {
            currentSlots.Add(new ItemSlot()
            {
                Item = item,
                Count = count
            });
        }

        OnUpdated?.Invoke();
    }

    /// <summary>
    /// Removes an item from the inventory, reducing the count of the item in the specified category. If the count reaches 0, the item is removed from the category.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="category">The category from which to remove the item.</param>
    public void RemoveItem(ItemBase item, int category)
    {
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.First(slot => slot.Item == item);
        itemSlot.Count--;
        if (itemSlot.Count == 0)
            currentSlots.Remove(itemSlot);

        OnUpdated?.Invoke();
    }

    /// <summary>
    /// Gets the category of an item based on its type.
    /// </summary>
    /// <param name="item">The item to get the category of.</param>
    /// <returns>The category of the item.</returns>
    ItemCategory GetCategoryFromItem(ItemBase item)
    {
        if (item is RecoveryItem)
            return ItemCategory.Medicines;
        else if (item is PokeballItem)
            return ItemCategory.Pokeballs;
        else
            return ItemCategory.Tms;
    }

    /// <summary>
    /// Gets the Inventory component of the PlayerController object.
    /// </summary>
    /// <returns>The Inventory component of the PlayerController object.</returns>
    public static Inventory GetInventory()
    {
        return FindObjectOfType<PlayerController>().GetComponent<Inventory>();
    }
}

/// <summary>
/// Represents an item slot in an inventory system.
/// </summary>
[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;

    /// <summary>
    /// Gets or sets the ItemBase object.
    /// </summary>
    public ItemBase Item
    {
        get => item;
        set => item = value;
    }

    /// <summary>
    /// Property to get and set the value of the count variable.
    /// </summary>
    public int Count
    {
        get => count;
        set => count = value;
    }
}