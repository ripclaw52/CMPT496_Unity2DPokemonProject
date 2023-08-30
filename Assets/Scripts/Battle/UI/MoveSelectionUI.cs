using GDEUtils.GenericSelectionUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelectionUI : SelectionUI<TextSlot>
{
    [SerializeField] List<TextSlot> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    private void Start()
    {
        SetSelectionSettings(SelectionType.Grid, 2);
    }

    List<Move> _moves;
    public void SetMoves(List<Move> moves)
    {
        _moves = moves;

        SetItems(moveTexts.Take(moves.Count).ToList());

        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < moves.Count)
                moveTexts[i].SetText(moves[i].Base.Name);
            else
                moveTexts[i].SetText("-");
        }
    }

    public override void UpdateSelectionInUI()
    {
        base.UpdateSelectionInUI();

        var move = _moves[selectedItem];

        ppText.text = $"PP {move.PP}/{move.Base.PP}";
        typeText.text = move.Base.Type.ToString();

        if (move.PP == 0)
            ppText.color = Color.red;
        else
            ppText.color = Color.black;
    }
}
