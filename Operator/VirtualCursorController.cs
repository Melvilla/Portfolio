using UnityEngine;
using UnityEngine.UI;

public class VirtualCursorController : MonoBehaviour
{
    [SerializeField] RectTransform canvasRect;
    [SerializeField] RectTransform cursorRect;
    [SerializeField] DynamicUIStage uiStage;

    private Vector2 virtualCursorPos = new Vector2(0.5f, 0.5f); // Normalized [0,1]
    private Vector3 lastMousePos;
    public float sensitivity = 1.0f; // Adjust for feel

    private void Start()
    {
        // Hide the system cursor
        Cursor.visible = false;
        // Initialize lastMousePos
#if ENABLE_LEGACY_INPUT_MANAGER
        lastMousePos = Input.mousePosition;
#else
        lastMousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#endif
    }

    private void Update()
    {
        // Get current mouse position
#if ENABLE_LEGACY_INPUT_MANAGER
        Vector3 mousePos = Input.mousePosition;
#else
        Vector3 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#endif

        // Calculate mouse delta
        Vector3 mouseDelta = mousePos - lastMousePos;
        lastMousePos = mousePos;

        // Convert mouse delta to normalized canvas space
        Vector2 canvasSize = canvasRect.sizeDelta;
        // Sensitivity scales how much the cursor moves per pixel of mouse movement
        virtualCursorPos += new Vector2(
            mouseDelta.x / canvasSize.x * sensitivity,
            mouseDelta.y / canvasSize.y * sensitivity
        );

        // Clamp to [0,1] so the cursor stays within the screen
        virtualCursorPos.x = Mathf.Clamp01(virtualCursorPos.x);
        virtualCursorPos.y = Mathf.Clamp01(virtualCursorPos.y);

        // Move the virtual cursor UI element
        Vector2 pixelPos = new Vector2(
            canvasSize.x * virtualCursorPos.x,
            canvasSize.y * virtualCursorPos.y
        );
        cursorRect.anchoredPosition = pixelPos;

        // Send input to UI system
        uiStage.OnCursorInput(virtualCursorPos);
    }
}
