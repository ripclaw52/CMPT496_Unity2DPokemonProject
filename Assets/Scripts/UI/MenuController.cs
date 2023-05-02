using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for controlling the menu UI elements.
/// </summary>
public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;

    public event Action<int> onMenuSelected;
    public event Action onBack;

    List<Text> menuItems;

    int selectedItem = 0;

    /// <summary>
    /// Gets all the Text components in the children of the menu object and stores them in a list. 
    /// </summary>
    private void Awake()
    {
        menuItems = menu.GetComponentsInChildren<Text>().ToList();
    }

    /// <summary>
    /// Opens the menu and updates the item selection.
    /// </summary>
    public void OpenMenu()
    {
        menu.SetActive(true);
        UpdateItemSelection();
    }

    /// <summary>
    /// Closes the menu by setting its active state to false.
    /// </summary>
    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    /// <summary>
    /// Handles the update of the menu, including selection changes and user input.
    /// </summary>
    public void HandleUpdate()
    {
        int prevSelection = selectedItem;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            ++selectedItem;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            --selectedItem;

        selectedItem = Mathf.Clamp(selectedItem, 0, menuItems.Count - 1);

        if (prevSelection != selectedItem)
            UpdateItemSelection();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            onMenuSelected?.Invoke(selectedItem);
            CloseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            onBack?.Invoke();
            CloseMenu();
        }
    }

    /// <summary>
    /// Iterates through the list of menu items and sets the color of the selected item to the highlighted color and all other items to black.
    /// </summary>
    void UpdateItemSelection()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (i == selectedItem)
                menuItems[i].color = GlobalSettings.i.HighlightedColor;
            else
                menuItems[i].color = Color.black;
        }
    }
}
