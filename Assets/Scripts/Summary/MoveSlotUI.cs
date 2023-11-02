using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveSlotUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI currentPPText;
    [SerializeField] TextMeshProUGUI maxPPText;

    [SerializeField] Image topImage;
    [SerializeField] Image bottomImage;
    [SerializeField] Image typeIcon;

    RectTransform rectTransform;


    public TextMeshProUGUI NameText => nameText;
    public TextMeshProUGUI CurrentPPText => currentPPText;
    public TextMeshProUGUI MaxPPText => maxPPText;
    public Image TopImage => topImage;
    public Image BottomImage => bottomImage;
    public Image TypeIcon => typeIcon;
    public float Height => rectTransform.rect.height;

    private void Awake()
    {
        
    }

    public void SetData(Move move)
    {
        rectTransform = GetComponent<RectTransform>();
        nameText.text = move.Base.Name;
        GetMovePP(move);

        TypeBase moveType = GlobalSettings.i.GetMoveType(move.Base);
        typeIcon.sprite = moveType.TypeIcon;
        typeIcon.color = Color.white;

        topImage.color = new Color(moveType.TypeColor.r, moveType.TypeColor.g, moveType.TypeColor.b, 0.75f);
        bottomImage.color = new Color(moveType.TypeColor.r, moveType.TypeColor.g, moveType.TypeColor.b, 0.5f);
    }

    void GetMovePP(Move move)
    {
        maxPPText.text = "/" + move.Base.PP.ToString();
        currentPPText.text = move.PP.ToString();
        var currentPP = move.PP;
        if ((currentPP / move.Base.PP) <= 0.5f)
        {
            currentPPText.color = Color.yellow;
        }
        else if ((currentPP / move.Base.PP) <= 0.25f)
        {
            currentPPText.color = Color.red;
        }
        else
        {
            currentPPText.color = Color.white;
        }
    }
}
