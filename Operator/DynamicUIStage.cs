using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DynamicUIStage : MonoBehaviour
{
    [SerializeField] RectTransform CanvasTransform;
    [SerializeField] GraphicRaycaster Raycaster;

    List<GameObject> DragTargets = new();

    public void OnCursorInput(Vector2 inNormalisedPosition)
    {
        // Get the input position in canvas space
        Vector3 InputPosition = new Vector3(CanvasTransform.sizeDelta.x * inNormalisedPosition.x,
                                           CanvasTransform.sizeDelta.y * inNormalisedPosition.y, 0);

        // Build a pointer event
        PointerEventData PointerEvent = new PointerEventData(EventSystem.current);
        PointerEvent.position = InputPosition;

        // Determine what we've hit in the UI
        List<RaycastResult> Results = new();
        Raycaster.Raycast(PointerEvent, Results);

#if ENABLE_LEGACY_INPUT_MANAGER
        bool bMouseDownThisFrame = Input.GetMouseButtonDown(0);
        bool bMouseUpThisFrame = Input.GetMouseButtonUp(0);
        bool bIsMouseDown = Input.GetMouseButton(0);
#else
        bool bMouseDownThisFrame = UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
        bool bMouseUpThisFrame = UnityEngine.InputSystem.Mouse.current.leftButton.wasReleasedThisFrame;
        bool bIsMouseDown = UnityEngine.InputSystem.Mouse.current.leftButton.isPressed;
#endif // ENABLE_LEGACY_INPUT_MANAGER

        // has the mouse button gone up this frame
        if (bMouseUpThisFrame)
        {
            foreach (var Target in DragTargets)
            {
                if (ExecuteEvents.Execute(Target, PointerEvent, ExecuteEvents.endDragHandler))
                {
                    break;
                }
            }
            DragTargets.Clear();
        }

        // Process any hit results
        foreach (var Result in Results)
        {
            // Create new event data
            PointerEventData PointerEventForResult = new PointerEventData(EventSystem.current);
            PointerEventForResult.position = InputPosition;
            PointerEventForResult.pointerCurrentRaycast = Result;
            PointerEventForResult.pointerPressRaycast = Result;

            // is the button down?
            if (bIsMouseDown)
            {
                PointerEventForResult.button = PointerEventData.InputButton.Left;
            }

            // Retrieve a slider if hit
            var HitSlider = Result.gameObject.GetComponentInParent<Slider>();

            // New drag targets?
            if (bMouseDownThisFrame)
            {
                if (ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.beginDragHandler))
                {
                    DragTargets.Add(Result.gameObject);
                }

                if (HitSlider != null)
                {
                    HitSlider.OnInitializePotentialDrag(PointerEventForResult);

                    if (!DragTargets.Contains(Result.gameObject))
                    {
                        DragTargets.Add(Result.gameObject);
                    }
                }
            }

            // Need to update current drag targets?
            else if (DragTargets.Contains(Result.gameObject))
            {
                PointerEventForResult.dragging = true;
                ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.dragHandler);

                if (HitSlider != null)
                {
                    HitSlider.OnDrag(PointerEventForResult);
                }
            }

            if (bMouseDownThisFrame)
            {
                if (ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.pointerDownHandler))
                {
                    break;
                }
            }
            else if (bMouseUpThisFrame)
            {
                bool bDidRun = false;
                bDidRun |= ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.pointerUpHandler);
                bDidRun |= ExecuteEvents.Execute(Result.gameObject, PointerEventForResult, ExecuteEvents.pointerClickHandler);

                if (bDidRun)
                    break;
            }
        }

    }
}
