using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Focus Points")]
    [SerializeField] private Transform restingPoint;
    [SerializeField] private float restingFov = 60f;

    [SerializeField] private Transform monitorPoint;
    [SerializeField] private float monitorFov = 40f;

    [SerializeField] private Transform caseboardPoint;
    [SerializeField] private float caseboardFov = 50f;

    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private float restingTransitionSpeed = 5f; 

    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 30f;
    [SerializeField] private float minYaw = -60f;
    [SerializeField] private float maxYaw = 60f;

    private float yaw = 0f;
    private float pitch = 0f;

    private Camera cam;
    private Transform targetPoint;
    private float targetFov;

    private Transform noteFocusPoint;
    private float noteFocusFov;

    public enum CameraState { Resting, Monitor, Caseboard, NoteZoom, TransitionToResting }
    public CameraState currentState = CameraState.Resting;

    void Start()
    {
        cam = GetComponent<Camera>();
        SetResting();
        yaw = 0f;
        pitch = 0f;
        UpdateCursorLock();
    }

    void Update()
    {
        if (currentState == CameraState.Resting)
        {
            HandleMouseLook();
        }
        else if (currentState == CameraState.TransitionToResting)
        {
            // Use the faster restingTransitionSpeed here
            transform.position = Vector3.Lerp(transform.position, restingPoint.position, Time.deltaTime * restingTransitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, restingPoint.rotation, Time.deltaTime * restingTransitionSpeed);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, restingFov, Time.deltaTime * restingTransitionSpeed);

            if (Vector3.Distance(transform.position, restingPoint.position) < 0.01f &&
                Quaternion.Angle(transform.rotation, restingPoint.rotation) < 0.5f &&
                Mathf.Abs(cam.fieldOfView - restingFov) < 0.1f)
            {
                // Sync yaw and pitch to restingPoint's rotation
                Vector3 restingEuler = restingPoint.rotation.eulerAngles;
                yaw = restingEuler.y;
                pitch = restingEuler.x;

                currentState = CameraState.Resting;
                UpdateCursorLock();
            }
        }
        else if (currentState == CameraState.NoteZoom && noteFocusPoint != null)
        {
            // Lerp position and rotation to the note's focus point, just like other transitions
            transform.position = Vector3.Lerp(transform.position, noteFocusPoint.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, noteFocusPoint.rotation, Time.deltaTime * transitionSpeed);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, noteFocusFov, Time.deltaTime * transitionSpeed);
        }
        else
        {
            // Use the normal transitionSpeed for other transitions
            transform.position = Vector3.Lerp(transform.position, targetPoint.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetPoint.rotation, Time.deltaTime * transitionSpeed);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * transitionSpeed);
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;

        yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.position = restingPoint.position;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void UpdateCursorLock()
    {
        if (currentState == CameraState.Resting)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Cursor locked and hidden (Resting)");
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Cursor unlocked and visible (Monitor/Caseboard)");
        }
    }

    public void SetResting()
    {
        // Start transition instead of instantly switching
        currentState = CameraState.TransitionToResting;
        UpdateCursorLock();
    }

    public void SetMonitorFocus()
    {
        targetPoint = monitorPoint;
        targetFov = monitorFov;
        currentState = CameraState.Monitor;
        UpdateCursorLock();
    }

    public void SetCaseBoardFocus()
    {
        targetPoint = caseboardPoint;
        targetFov = caseboardFov;
        currentState = CameraState.Caseboard;
        UpdateCursorLock();
    }

    public void SetNoteFocus(Transform noteTransform, float fov)
    {
        noteFocusPoint = noteTransform;
        noteFocusFov = fov;
        currentState = CameraState.NoteZoom;
        UpdateCursorLock();
    }
}
