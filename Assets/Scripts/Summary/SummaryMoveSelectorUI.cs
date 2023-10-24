using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryMoveSelectorUI : MonoBehaviour, ISelectableItem
{
    [SerializeField] TextMeshProUGUI moveName;
    [SerializeField] TextMeshProUGUI currentMovePP;
    [SerializeField] TextMeshProUGUI maxMovePP;
    
    [SerializeField] Image backgroundImage;
    [SerializeField] Image typeIcon;

    Move baseMove;

    public void Init(Move move)
    {
        baseMove = move;

        moveName.text = move.Base.Name;
        CheckMoveType(move);
        GetMovePP(move);
    }

    public void Init()
    {

    }

    public void Clear()
    {

    }

    public void OnSelectionChanged(bool selected)
    {
        foreach (var item in GlobalSettings.i.Type)
        {
            if (item.Type == baseMove.Base.Type)
            {
                backgroundImage.color = (selected) ? new Color(item.TypeColor.r, item.TypeColor.g, item.TypeColor.b, 125f) : new Color(item.TypeColor.r, item.TypeColor.g, item.TypeColor.b, 255f);
            }
        }
    }

    public void CheckMoveType(Move move)
    {
        foreach (var item in GlobalSettings.i.Type)
        {
            if (item.Type == move.Base.Type)
            {
                typeIcon.sprite = item.TypeIcon;
                backgroundImage.color = new Color(item.TypeColor.r, item.TypeColor.g, item.TypeColor.b, 125f);
            }
        }
    }

    public void GetMovePP(Move move)
    {
        maxMovePP.text = "/" + move.Base.PP.ToString();
        currentMovePP.text = move.PP.ToString();
        var currentPP = move.PP;
        if (currentPP / move.Base.PP <= 0.5f)
        {
            currentMovePP.color = Color.yellow;
        }
        else if (currentPP / move.Base.PP <= 0.25f)
        {
            currentMovePP.color = Color.red;
        }
        else
        {
            currentMovePP.color = Color.white;
        }
    }
}
