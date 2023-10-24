using GDEUtils.GenericSelectionUI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryMoveUI : SelectionUI<SummaryMoveSelectorUI>
{
    [SerializeField] GameObject prefabMoveSelector;
    [SerializeField] GameObject prefabMoveSelectionParent;

    [SerializeField] GameObject moveInfo;
    [SerializeField] GameObject interactionInfo;

    [SerializeField] TextMeshProUGUI moveNameText;

    [SerializeField] Image moveTypeIcon;
    [SerializeField] Image moveCategoryIcon;

    [SerializeField] TextMeshProUGUI movePowerText;
    [SerializeField] TextMeshProUGUI moveAccuracyText;
    [SerializeField] TextMeshProUGUI moveDescriptionText;

    List<Move> moveList = new List<Move>();
    int selectedMove = 0;

    float selectionTimer = 0;
    const float selectionSpeed = 5;

    //public event Action<int> OnSelected;
    public event Action OnBack;
    
    public void Init(Pokemon pokemon)
    {
        moveList.Clear();
        foreach (Transform child in prefabMoveSelectionParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var move in pokemon.Moves)
        {
            var newMove = Instantiate(prefabMoveSelector);
            newMove.GetComponent<SummaryMoveSelectorUI>().Init(move);
            newMove.transform.SetParent(prefabMoveSelectionParent.transform);

            moveList.Add(move);
        }

        moveInfo.SetActive(false);
        interactionInfo.SetActive(true);
    }

    public void InitializeMoveBox()
    {
        MoveInfoInit(moveList[0].Base);
    }

    public override void HandleUpdate()
    {
        UpdateSelectionTimer();
        int prevSelection = selectedMove;

        float v = Input.GetAxis("Vertical");

        if (selectionTimer == 0 && Mathf.Abs(v) > 0.2f)
        {
            selectedMove += -(int)Mathf.Sign(v);
            selectionTimer = 1 / selectionSpeed;
            AudioManager.i.PlaySfx(AudioId.UISelect);
        }

        if (selectedMove > moveList.Count - 1)
        {
            selectedMove = 0;
            prevSelection = moveList.Count - 1;
        }
        else if (selectedMove < 0)
        {
            selectedMove = moveList.Count - 1;
            prevSelection = 0;
        }

        Debug.Log($"index={selectedMove}");

        if (selectedMove != prevSelection)
        {
            moveInfo.SetActive(true);
            interactionInfo.SetActive(false);
            //Debug.Log($"{moveList[selectedMove].Base.Name}");
            MoveInfoInit(moveList[selectedMove].Base);
        }

        if (Input.GetButtonDown("Back"))
        {
            AudioManager.i.PlaySfx(AudioId.UICancel);
            OnBack?.Invoke();
        }
    }

    void MoveInfoInit(MoveBase move)
    {
        moveNameText.text = move.Name;
        moveDescriptionText.text = move.Description;

        movePowerText.text = move.Power.ToString();
        moveAccuracyText.text = move.Accuracy.ToString();

        if (move.Power == 0)
            movePowerText.text = " - ";

        if (move.Accuracy == 0)
            moveAccuracyText.text = " - ";

        if (move.Category == MoveCategory.Status)
        {
            if (move.Power == 0)
                movePowerText.text = " - ";
        }

        if (move.AlwaysHits)
            moveAccuracyText.text = " - ";
    }

    void UpdateSelectionTimer()
    {
        if (selectionTimer > 0)
            selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
    }
}
