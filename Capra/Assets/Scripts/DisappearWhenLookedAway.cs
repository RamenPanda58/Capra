using UnityEngine;

public class DisappearWhenLookedAway : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;   // Assign your player camera
    public GameObject capra;      // Assign Capra (the character that should disappear)

    [Header("Settings")]
    public float lookThreshold = 30f; // How far the player can look away before Capra disappears

    private bool checkActive = false;

    void Update()
    {
        if (!checkActive || playerCamera == null || capra == null)
            return;

        // Calculate angle between camera forward and direction to Capra
        Vector3 directionToCapra = (capra.transform.position - playerCamera.transform.position).normalized;
        float angle = Vector3.Angle(playerCamera.transform.forward, directionToCapra);

        // If player looks away past the threshold
        if (angle > lookThreshold)
        {
            capra.SetActive(false); // Make Capra disappear
            checkActive = false;    // Stop checking after disappearing
        }
    }

    //  Call this from the End Dialogue event in the Inspector
    public void OnEndDialogue()
    {
        checkActive = true;
    }
}
