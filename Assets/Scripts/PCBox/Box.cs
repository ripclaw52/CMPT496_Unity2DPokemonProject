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
    [SerializeField] string boxName = "box";
    [SerializeField] Image backgroundImage;

    // creates box with 30 items initially

    // none value
    [SerializeField] List<Pokemon> boxList = Enumerable.Repeat<Pokemon>(new Pokemon(), 30).ToList();

    public event Action OnUpdated;

    int listSize;
    int pokemonInBox;
    bool isFull;

    public Box(Box box)
    {
        this.boxName = box.boxName;
        this.backgroundImage = box.backgroundImage;
        this.boxList = box.boxList;
        listSize = boxList.Count;
    }

    public Box(string boxName)
    {
        this.boxName = boxName;
        boxList = new List<Pokemon?>(30);
        listSize = boxList.Count;
    }

    public Box(string boxName, Image background)
    {
        this.boxName = boxName;
        backgroundImage.sprite = background.sprite;
        boxList = new List<Pokemon?>(30);
        listSize = boxList.Count;
    }

    public Box()
    {
        boxList = new List<Pokemon>(30);
        listSize = boxList.Count;
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
        BoxName = saveData.name;
        BoxList = saveData.list.Select(p => new Pokemon(p)).ToList();
        
        Debug.Log($"Figure something else out for storing images. Use GlobalSettings and Image dictionary for lookup?");
        //BackgroundImage = saveData.background;
    }

    public BoxSaveData GetSaveData()
    {
        var saveData = new BoxSaveData()
        {
            name = BoxName,
            list = BoxList.Select(p => p.GetSaveData()).ToList(),

            //background = BackgroundImage,
        };

        return saveData;
    }

    public string BoxName { get => boxName; set => boxName = value; }
    public List<Pokemon> BoxList
    {
        get { return boxList; }
        set { boxList = value; OnUpdated?.Invoke(); }
    }
    public Image BackgroundImage { get => backgroundImage; set => backgroundImage = value; }

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
    //public Image background;
    public List<PokemonSaveData> list;
}