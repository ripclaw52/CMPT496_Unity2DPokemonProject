using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneAction
{
    [SerializeField] string name;

    public string Name
    {
        get => name;
        set => name = value;
    }
}
