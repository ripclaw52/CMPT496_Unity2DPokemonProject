using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [TextArea]
    [SerializeField] string usedMessage;
    [SerializeField] Sprite icon;

    public string Name => name;
    public string Description => description;
    public string UsedMessage => usedMessage;
    public Sprite Icon => icon;

    public virtual bool Use(Pokemon pokemon)
    {
        return false;
    }
}
