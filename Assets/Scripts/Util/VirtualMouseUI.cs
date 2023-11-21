using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    [HideInInspector] public VirtualMouseInput virtualMouseInput;

    public static VirtualMouseUI i { get; private set; }

    private void Awake()
    {
        i = this;
        virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    private void Update()
    {
        transform.localScale = Vector3.one * (1f / canvasRectTransform.localScale.x);
        transform.SetAsLastSibling();
    }

    private void LateUpdate()
    {
        Vector2 virtualMousePosition = virtualMouseInput.virtualMouse.position.ReadValue();
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0f, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0f, Screen.height);
        InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePosition);
    }

    public void MoveMousePosition()
    {
        // Top Left corner
        virtualMouseInput.cursorTransform.position = new Vector3(35f, Screen.height - 35f, 0f);
    }

    public VirtualMouseInput VirtualMouseInput => virtualMouseInput;
}
