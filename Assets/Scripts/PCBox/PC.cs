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

    private void Awake()
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