using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a database of items.
/// </summary>
public class ItemDB
{
    static Dictionary<string, ItemBase> items;

    /// <summary>
    /// Initializes the items dictionary by loading all ItemBase objects from the Resources folder. 
    /// If two items have the same name, an error is logged.
    /// </summary>
    public static void Init()
    {
        items = new Dictionary<string, ItemBase>();

        var itemList = Resources.LoadAll<ItemBase>("");
        foreach (var item in itemList)
        {
            if (items.ContainsKey(item.Name))
            {
                Debug.LogError($"There are two items with the name {item.Name}!");
                continue;
            }

            items[item.Name] = item;
        }
    }

    /// <summary>
    /// Retrieves an item from the database by its name.
    /// </summary>
    /// <param name="name">The name of the item to retrieve.</param>
    /// <returns>The item with the specified name, or null if not found.</returns>
    public static ItemBase GetItemByName(string name)
    {
        if (!items.ContainsKey(name))
        {
            Debug.LogError($"Item with name {name} not found in the database!");
            return null;
        }

        return items[name];
    }
}
