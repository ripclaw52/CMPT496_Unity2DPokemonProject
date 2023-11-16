using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCUI : MonoBehaviour
{
    [SerializeField] Image leftArrow;
    [SerializeField] Image rightArrow;

    int selectedBoxIndex = 0;

    public void GoToNextBox()
    {
        if (PC.i.PCList.Count == 0)
        {
            Debug.Log("Empty List");
            return;
        }
        
        Debug.Log($"next={selectedBoxIndex} befor");
        selectedBoxIndex = (selectedBoxIndex == PC.i.PCList.Count - 1) ? 0 : selectedBoxIndex + 1;
        Debug.Log($"next={selectedBoxIndex} after");
    }

    public void GoToPrevBox()
    {
        if (PC.i.PCList.Count == 0)
        {
            Debug.Log("Empty List");
            return;
        }

        Debug.Log($"prev={selectedBoxIndex} befor");
        selectedBoxIndex = (selectedBoxIndex == 0) ? PC.i.PCList.Count - 1 : selectedBoxIndex - 1;
        Debug.Log($"prev={selectedBoxIndex} after");
    }
}
