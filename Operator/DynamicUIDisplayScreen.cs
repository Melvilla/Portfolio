using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DynamicUIDisplayScreen : MonoBehaviour
{
    [SerializeField] LayerMask RaycastMask = ~0;
    [SerializeField] float RaycastDistance = 5.0f;
    [SerializeField] UnityEvent<Vector2> OnCursorInput = new();

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.None && Cursor.visible)
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            Vector3 MousePosition = Input.mousePosition;
#else
        Vector3 MousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#endif // ENABLE_LEGACY_INPUT_MANAGER

            // Construct our ray from the mouse position
            Ray MouseRay = Camera.main.ScreenPointToRay(MousePosition);

            // Perform the raycase
            RaycastHit HitResult;
            if (Physics.Raycast(MouseRay, out HitResult, RaycastDistance, RaycastMask, QueryTriggerInteraction.Ignore))
            {
                // Ignore if not us
                if (HitResult.collider.gameObject != gameObject)
                    return;

                OnCursorInput.Invoke(HitResult.textureCoord);
            }
        }
    }
}
