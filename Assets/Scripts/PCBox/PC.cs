using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
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
        OnUpdated?.Invoke();
    }

    public static PC GetPC()
    {
        return FindObjectOfType<PlayerController>().GetComponent<PC>();
    }
}
