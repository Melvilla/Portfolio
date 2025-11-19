using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ComputerUIManager computerUIManager;
    // Add caseboard references here

    void Update()
    {
        if (cameraController.currentState == CameraController.CameraState.Resting)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                // Draw the ray in the Scene view for debugging
                Debug.DrawRay(ray.origin, ray.direction * 5f, Color.green, 1f);

                if (Physics.Raycast(ray, out RaycastHit hit, 5f))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        switch (interactable.FocusType)
                        {
                            case Interactable.CameraFocusType.Monitor:
                                cameraController.SetMonitorFocus();
                                break;
                            case Interactable.CameraFocusType.Caseboard:
                                cameraController.SetCaseBoardFocus();
                                break;
                            case Interactable.CameraFocusType.Note:
                                if (interactable.NoteFocusPoint != null)
                                    cameraController.SetNoteFocus(interactable.NoteFocusPoint, interactable.NoteFocusFov);
                                break;
                        }
                    }
                }
            }
        }

        if (cameraController.currentState == CameraController.CameraState.Monitor ||
            cameraController.currentState == CameraController.CameraState.Caseboard ||
            cameraController.currentState == CameraController.CameraState.NoteZoom)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                cameraController.SetResting();
            }
        }
    }

}
