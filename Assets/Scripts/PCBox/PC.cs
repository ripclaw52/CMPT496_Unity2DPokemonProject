using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    [SerializeField] List<Box> pcList = new List<Box>();
    public List<Box> PCList { get => pcList; set => pcList = value; }

    public static PC i { get; set; }
    private void Awake()
    {
        i = this;
    }

    void AddBox()
    {
        pcList.Add(new Box($"box_{pcList.Count}"));
    }
}
