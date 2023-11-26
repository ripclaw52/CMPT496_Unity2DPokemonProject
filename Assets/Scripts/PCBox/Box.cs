using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Box
{
    [SerializeField] BoxName boxType = BoxName.None;
    [SerializeField] string boxHeaderName = "";

    // creates box with 30 items initially

    // none value
    [SerializeField] List<Pokemon> boxList = Enumerable.Repeat<Pokemon>(new Pokemon(), 30).ToList();

    public event Action OnUpdated;

    int listSize;
    int pokemonInBox;
    bool isFull;

    public Box(BoxName type, string headerName)
    {
        boxList = Enumerable.Repeat<Pokemon>(new Pokemon(), 30).ToList();
        boxType = type;
        boxHeaderName = headerName;

        Init();
    }

    public void Init()
    {
        for (int i = 0; i < boxList.Count; i++)
        {
            if (boxList[i].HasValue != false)
            {
                boxList[i].Init();
            }
        }
    }

    int GetAmountOfPokemonInBox()
    {
        int total = 0;
        foreach (var item in BoxList)
        {
            if (item.HasValue)
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
            if (item.HasValue)
            {
                newList.Add(item);
            }
        }
        return newList;
    }

    // Adds a pokemon to the first index in the list containing null value
    public void AddPokemon(Pokemon pokemon)
    {
        for (int i = 0; i < BoxList.Count; i++)
        {
            // has value is false
            if (!BoxList[i].HasValue)
            {
                BoxList[i] = pokemon;
                return;
            }
        }
    }

    public void BoxUpdated()
    {
        OnUpdated?.Invoke();
    }

    // Add save system
    public Box(BoxSaveData saveData)
    {
        BoxHeaderName = saveData.name;
        BoxType = saveData.boxType;
        BoxList = saveData.list.Select(p => new Pokemon(p)).ToList();
    }

    public BoxSaveData GetSaveData()
    {
        var saveData = new BoxSaveData()
        {
            name = BoxHeaderName,
            boxType = BoxType,
            list = BoxList.Select(p => p.GetSaveData()).ToList(),

            //background = BackgroundImage,
        };

        return saveData;
    }

    public string BoxHeaderName { get => boxHeaderName; set => boxHeaderName = value; }
    public BoxName BoxType { get => boxType; set => boxType = value; }
    public List<Pokemon> BoxList
    {
        get { return boxList; }
        set { boxList = value; OnUpdated?.Invoke(); }
    }

    public int PokemonInBox
    {
        get { pokemonInBox = GetAmountOfPokemonInBox(); return pokemonInBox; }
    }
    public bool IsFull
    {
        get { isFull = (PokemonInBox == 30); return isFull; }
    }
}

[System.Serializable]
public class BoxSaveData
{
    public string name;
    public BoxName boxType;
    public List<PokemonSaveData> list;
}