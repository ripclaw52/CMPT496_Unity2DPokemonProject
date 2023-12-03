using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PC : MonoBehaviour, ISavable
{
    [SerializeField] List<Box> pcList;
    public event Action OnUpdated;
    public List<Box> PCList
    {
        get
        {
            return pcList;
        }
        set
        {
            pcList = value;
            OnUpdated?.Invoke();
        }
    }

    private void Start()
    {
        foreach (var box in pcList)
        {
            box.Init();
        }
    }

    public void PCUpdated()
    {
        foreach (var box in pcList)
        {
            box.BoxUpdated();
        }

        OnUpdated?.Invoke();
    }

    public void AddMoreBoxes()
    {
        int listCount = PCList.Count();
        int fullBoxes = 0;
        foreach (var box in PCList)
        {
            if (box.IsFull)
                fullBoxes++;
        }

        // while 4 boxes are free
        if ((fullBoxes + 4) >= listCount)
        {
            for (int i = 0; i < 4; i++)
            {
                int index = listCount % 16;
                int nameMod = listCount / 16;

                BoxImageData bid = GlobalSettings.i.Boxes[index];
                string headerName = bid.GetBoxNameString();
                if (nameMod != 0)
                {
                    headerName = $"{bid.GetBoxNameString()}_{nameMod}";
                }

                Debug.Log($"{i} _ {headerName}");

                Box box = new Box(bid.BoxType, headerName);
                PCList.Add(box);

                listCount = PCList.Count();
            }
        }
    }

    public void AddPokemonIntoBox(Pokemon pokemon)
    {
        // check if can add more boxes
        AddMoreBoxes();

        foreach (var box in PCList)
        {
            if (!box.IsFull)
            {
                box.AddPokemon(pokemon);
                return;
            }
        }
    }

    public void ReadThroughBox(int index)
    {
        foreach (var pokemon in PCList[index].BoxList)
        {
            Debug.Log($"CheckValue of Pokemon({pokemon?.HasValue})");
        }
    }

    public object CaptureState()
    {
        var saveData = new PCSaveData()
        {
            boxes = PCList.Select(p => p.GetSaveData()).ToList(),
        };

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = (PCSaveData)state;

        PCList = saveData.boxes.Select(p => new Box(p)).ToList();
    }

    public static PC GetPC()
    {
        return FindObjectOfType<PlayerController>().GetComponent<PC>();
    }
}

[System.Serializable]
public class PCSaveData
{
    public List<BoxSaveData> boxes;
}