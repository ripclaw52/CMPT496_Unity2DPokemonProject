using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Box
{
    [SerializeField] string boxName = "box";
    [SerializeField] Image backgroundImage;

    // creates box with 30 items initially
    [SerializeField] List<Pokemon?> boxList = new List<Pokemon?>(Enumerable.Repeat((Pokemon?)null, 30));
    int listSize;
    int fillAmount;
    bool isFull;

    public Box(string boxName)
    {
        this.boxName = boxName;
        boxList = new List<Pokemon?>(Enumerable.Repeat((Pokemon?)null, 30));
        listSize = boxList.Count;
        GetFilledAmount();
    }

    public Box(string boxName, Image background)
    {
        this.boxName = boxName;
        backgroundImage.sprite = background.sprite;
        boxList = new List<Pokemon?>(Enumerable.Repeat((Pokemon?)null, 30));
        listSize = boxList.Count;
        GetFilledAmount();
    }

    public Box()
    {
        boxList = new List<Pokemon?>(Enumerable.Repeat((Pokemon?)null, 30));
        listSize = boxList.Count;
        GetFilledAmount();
    }

    int GetFilledAmount()
    {
        int total = 0;
        foreach (var item in BoxList)
        {
            if (item != null)
                total++;
        }
        return total;
    }

    // Get human readable list and strips null fields from box list
    public List<Pokemon> GetReadableList()
    {
        List<Pokemon> newList = new List<Pokemon>();
        foreach (var item in BoxList)
        {
            if (item != null)
            {
                newList.Add(item);
            }
        }
        return newList;
    }

    // Adds a pokemon to the first index in the list containing null value
    public void AddPokemon(Pokemon pokemon)
    {
        GetFilledAmount();
        for (int i = 0; i < ListSize; i++)
        {
            if (BoxList[i] == null)
            {
                BoxList[i] = pokemon;
                return;
            }
        }
    }

    /// <summary>
    /// Moves a Pokemon into the current index
    /// </summary>
    /// <param name="pokemon">The pokemon being added into the box list</param>
    /// <param name="index">The index the pokemon is being added into</param>
    /// <returns>This returns the value at the previous index, either null or a pokemon</returns>
    public Pokemon? MovePokemon(Pokemon pokemon, int index)
    {
        GetFilledAmount();
        Pokemon? prevPokemon = BoxList[index];
        BoxList[index] = pokemon;
        return prevPokemon;
    }

    // Add save system

    public string BoxName { get => boxName; set => boxName = value; }
    public List<Pokemon?> BoxList { get => boxList; set => boxList = value; }
    public Image BackgroundImage { get => backgroundImage; set => backgroundImage = value; }
    public int ListSize => listSize;
    public int FillAmount { get => fillAmount; set => fillAmount = GetFilledAmount(); }
    public bool IsFull { get => isFull = (fillAmount == 30) ? true : false; }
}
