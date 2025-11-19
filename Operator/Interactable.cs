using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum CameraFocusType { Resting, Monitor, Caseboard, Note }

    [SerializeField] private CameraFocusType focusType;

    // For notes: assign a focus point and FOV in the inspector
    [SerializeField] private Transform noteFocusPoint;
    [SerializeField] private float noteFocusFov = 30f;

    public CameraFocusType FocusType => focusType;
    public Transform NoteFocusPoint => noteFocusPoint;
    public float NoteFocusFov => noteFocusFov;

}
