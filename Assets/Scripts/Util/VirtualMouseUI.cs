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

    public void SetToCenterOfScreen()
    {
        Debug.Log($"mouse before pos? {virtualMouseInput.virtualMouse.position.ReadValue()}");
        Vector2 center = new Vector2(canvasRectTransform.localPosition.x / 2f, canvasRectTransform.localPosition.y / 2f);

        //InputState.Change(virtualMouseInput.virtualMouse.position, center);
        Debug.Log($"mouse after pos? {virtualMouseInput.virtualMouse.position.ReadValue()}");
    }

    public VirtualMouseInput VirtualMouseInput => virtualMouseInput;
}
