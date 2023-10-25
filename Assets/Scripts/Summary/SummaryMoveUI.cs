using GDEUtils.GenericSelectionUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryMoveUI : SelectionUI<MoveSlot>
{
    [SerializeField] GameObject moveSlotList;
    [SerializeField] MoveSlotUI moveSlotUI;

    //[SerializeField] GameObject prefabMoveSelector;
    //[SerializeField] GameObject prefabMoveSelectionParent;

    [SerializeField] GameObject moveInfo;
    [SerializeField] GameObject interactionInfo;

    [SerializeField] TextMeshProUGUI moveNameText;
    [SerializeField] Image moveTypeIcon;
    [SerializeField] Image moveCategoryIcon;
    [SerializeField] TextMeshProUGUI movePowerText;
    [SerializeField] TextMeshProUGUI moveAccuracyText;
    [SerializeField] TextMeshProUGUI moveDescriptionText;

    List<Move> moveList;
    List<MoveSlotUI> slotUIList;
    RectTransform moveListRect;

    int selectedMove = 0;

    private float selectionMoveUITimer = 0;
    const float selectionSpeed = 5;

    public GameObject MoveInfo => moveInfo;
    public GameObject InteractionInfo => interactionInfo;

    private void Awake()
    {
        moveListRect = moveSlotList.GetComponent<RectTransform>();
        moveInfo.SetActive(false);
        interactionInfo.SetActive(true);
    }

    public void Init(Pokemon pokemon)
    {
        //Debug.Log($"Init called in summaryMoveUI");

        if (moveList != null) moveList.Clear();
        moveList = new List<Move>(pokemon.Moves);

        UpdateMoveList();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameController.Instance.StateMachine.Push(SummaryMoveState.i);
            SetSelectionSettings(SelectionType.ListV, 1);
        }
    }

    void UpdateMoveList()
    {
        foreach (Transform child in moveSlotList.transform)
            Destroy(child.gameObject);

        slotUIList = new List<MoveSlotUI>();
        foreach (var move in moveList)
        {
            var slotUIObj = Instantiate(moveSlotUI, moveSlotList.transform);
            slotUIObj.SetData(move);

            slotUIList.Add(slotUIObj);
        }

        // Handles pokemon knowing less than 4 moves
        int moveListCount = moveList.Count;
        while (moveListCount < 4)
        {
            Instantiate(moveSlotUI, moveSlotList.transform);
            moveListCount++;
        }

        SetItems(slotUIList.Select(s => s.GetComponent<MoveSlot>()).ToList());

        UpdateSelectionInUI();
    }

    public void InitializeMoveBox()
    {
        //Debug.Log($"{slotUIList[0]}");
        MoveInfoInit(moveList[0].Base);
    }

    public override void HandleUpdate()
    {
        UpdateSelectionTimer();
        int prevSelection = selectedMove;

        // Ensure selectedMove var matches selectedItem from inherited selectionUI
        //Debug.Log($"move;{selectedMove}, itm;{selectedItem}");

        // vertical move selection
        float v = Input.GetAxis("Vertical");
        if (selectionMoveUITimer == 0 && Mathf.Abs(v) > 0.2f)
        {
            selectedMove += -(int)Mathf.Sign(v);
            selectionMoveUITimer = 1 / selectionSpeed;
            AudioManager.i.PlaySfx(AudioId.UISelect);
        }

        // looping conditions
        if (selectedMove > moveList.Count - 1)
            selectedMove = 0;
        else if (selectedMove < 0)
            selectedMove = moveList.Count - 1;

        // Update right panel with detailed move information
        //Debug.Log($"index={selectedMove}");
        if (selectedMove != prevSelection)
        {
            MoveInfoInit(moveList[selectedMove].Base);
        }

        base.HandleUpdate();
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
        if (selectionMoveUITimer > 0)
            selectionMoveUITimer = Mathf.Clamp(selectionMoveUITimer - Time.deltaTime, 0, selectionMoveUITimer);
    }
}
