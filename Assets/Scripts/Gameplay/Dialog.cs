using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store dialog data.
/// </summary>
[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> lines;
    public List<string> Lines => lines;
}
