using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDEUtils.GenericSelectionUI
{
    public enum AxisType { Vertical, Horizontal }
    public enum SelectionType { ListH, ListV, Grid }

    public class SelectionUI<T> : MonoBehaviour where T : ISelectableItem
    {
        List<T> items;
        protected int selectedItem = 0;

        SelectionType selectionType;
        int gridWidth = 2;

        float selectionTimer = 0;

        const float selectionSpeed = 5;

        public event Action<int> OnSelected;
        public event Action OnBack;

        public void SetSelectionSettings(SelectionType selectionType, int gridWidth)
        {
            this.selectionType = selectionType;
            this.gridWidth = gridWidth;
        }

        public void SetItems(List<T> items)
        {
            this.items = items;

            items.ForEach(i => i.Init());
            UpdateSelectionInUI();
        }

        public void ClearItems()
        {
            items.ForEach(i => i.Clear());

            this.items = null;
        }

        public virtual void HandleUpdate()
        {
            UpdateSelectionTimer();
            int prevSelection = selectedItem;

            if (selectionType == SelectionType.ListH)
                HandleListSelection(AxisType.Horizontal);
            else if (selectionType == SelectionType.ListV)
                HandleListSelection(AxisType.Vertical);
            else if (selectionType == SelectionType.Grid)
                HandleGridSelection();

            //selectedItem = Mathf.Clamp(selectedItem, 0, items.Count - 1);
            if (selectedItem < 0)
                selectedItem = items.Count - 1;
            else if (selectedItem > items.Count - 1)
                selectedItem = 0;

            if (selectedItem != prevSelection)
                UpdateSelectionInUI();

            if (Input.GetButtonDown("Action"))
            {
                AudioManager.i.PlaySfx(AudioId.UIConfirm);
                OnSelected?.Invoke(selectedItem);
            }
            else if (Input.GetButtonDown("Back"))
            {
                AudioManager.i.PlaySfx(AudioId.UICancel);
                OnBack?.Invoke();
            }
        }

        void HandleListSelection(AxisType axis)
        {
            float v = Input.GetAxis(axis.ToString());

            if (selectionTimer == 0 && Mathf.Abs(v) > 0.2f)
            {
                selectedItem += -(int)Mathf.Sign(v);

                selectionTimer = 1 / selectionSpeed;

                AudioManager.i.PlaySfx(AudioId.UISelect);
            }
        }

        void HandleGridSelection()
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            if (selectionTimer == 0 && (Mathf.Abs(v) > 0.2f || Mathf.Abs(h) > 0.2f))
            {
                if (Mathf.Abs(h) > Mathf.Abs(v))
                    selectedItem += (int)Mathf.Sign(h);
                else
                    selectedItem += -(int)Mathf.Sign(v) * gridWidth;

                selectionTimer = 1 / selectionSpeed;

                AudioManager.i.PlaySfx(AudioId.UISelect);
            }
        }

        public virtual void UpdateSelectionInUI()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].OnSelectionChanged(i == selectedItem);
            }
        }

        void UpdateSelectionTimer()
        {
            if (selectionTimer > 0)
                selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
        }
    }
}
