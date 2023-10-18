using GDEUtils.GenericSelectionUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryUI : MonoBehaviour
{
    [SerializeField] List<GameObject> pageList;
    Pokemon selectedPokemon;

    public int selectedPage = 0;

    float selectionTimer = 0;
    const float selectionSpeed = 5;

    public event Action<int> OnSelected;
    public event Action OnBack;

    public void HandleUpdate()
    {
        UpdateSelectionTimer();
        int prevSelection = selectedPage;

        float h = Input.GetAxis("Horizontal");
        if (selectionTimer == 0 && Mathf.Abs(h) > 0.2f)
        {
            selectedPage += (int)Mathf.Sign(h);
            selectionTimer = 1 / selectionSpeed;
            AudioManager.i.PlaySfx(AudioId.UISelect);
        }

        if (selectedPage > pageList.Count - 1)
        {
            selectedPage = 0;
            prevSelection = pageList.Count - 1;
        }
        else if (selectedPage < 0)
        {
            selectedPage = pageList.Count - 1;
            prevSelection = 0;
        }

        if (selectedPage != prevSelection)
        {
            pageList[prevSelection].SetActive(false);
            pageList[selectedPage].SetActive(true);
        }

        if (Input.GetButtonDown("Action"))
        {
            AudioManager.i.PlaySfx(AudioId.UIConfirm);
            OnSelected?.Invoke(selectedPage);
        }
        else if (Input.GetButtonDown("Back"))
        {
            AudioManager.i.PlaySfx(AudioId.UICancel);
            OnBack?.Invoke();
        }
    }

    void UpdateSelectionTimer()
    {
        if (selectionTimer > 0)
            selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
    }
}
