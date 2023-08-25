using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectableItem
{
    void Init();
    void OnSelectionChanged(bool selected);
}
