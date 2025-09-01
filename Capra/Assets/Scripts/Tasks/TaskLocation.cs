using UnityEngine;

public class TaskLocation : MonoBehaviour
{
    [SerializeField] private string locationName;
    public string LocationName => locationName;

    public static TaskLocation CurrentLocation { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            CurrentLocation = this;
            Debug.Log("Player detected");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && CurrentLocation == this)
            CurrentLocation = null;
    }
}
