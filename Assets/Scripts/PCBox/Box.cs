using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Box
{
    string boxName;

    // creates box with 30 items initially
    List<Pokemon?> boxList = new List<Pokemon?>(Enumerable.Repeat((Pokemon?)null, 30));
    int listSize;
    bool isFull;

    public Box()
    {
        listSize = boxList.Count;
    }

    public void CheckIsFull()
    {
        int total = 0;
        foreach (var item in BoxList)
        {
            if (item != null)
                total++;
        }
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
        Pokemon? prevPokemon = BoxList[index];
        BoxList[index] = pokemon;
        return prevPokemon;
    }

    // Add save system

    public string BoxName { get => boxName; set => boxName = value; }
    public List<Pokemon?> BoxList { get => boxList; set=> boxList = value; }
    public int ListSize => listSize;
    public bool IsFull => isFull;
}
