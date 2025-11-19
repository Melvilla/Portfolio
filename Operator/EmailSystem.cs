using UnityEngine;

public class EmailSystem : MonoBehaviour
{
    [Header("Email Prefabs")]
    [SerializeField] GameObject[] EmailPrefabs;

    [Header("Scroll View Content")]
    [SerializeField] Transform EmailContentParent; // The content transform in the EmailScreen

    private int currentEmailIndex = 0;

    private void Update()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetKeyDown(KeyCode.Space))
#else
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
#endif // ENABLE_LEGACY_INPUT_MANAGER
        {
            AddNextEmail();
        }
    }

    private void AddNextEmail()
    {
        if (EmailPrefabs == null|| EmailPrefabs.Length == 0 || EmailContentParent == null)
                return;

        // Loop through emails for testing
        var prefab = EmailPrefabs[currentEmailIndex % EmailPrefabs.Length]; // % = modulus operator to loop through array repeatedly
        var emailGO = Instantiate(prefab, EmailContentParent);
        emailGO.transform.SetAsLastSibling();

        currentEmailIndex++;

    }
}
