using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayOnActive : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        // Cache the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // Play the audio every time the object becomes active
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}