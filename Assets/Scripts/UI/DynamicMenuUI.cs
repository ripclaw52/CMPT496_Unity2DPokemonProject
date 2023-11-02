using GDEUtils.GenericSelectionUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMenuUI : SelectionUI<TextSlot>
{
    private void Start()
    {
        SetSelectionSettings(SelectionType.ListV, 1);
    }
}
