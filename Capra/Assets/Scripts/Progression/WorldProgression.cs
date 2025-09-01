using UnityEngine;

public class WorldProgression : MonoBehaviour
{
    public static WorldProgression Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Apply world changes based on a reward code
    public void ApplyReward(string rewardCode)
    {
        switch (rewardCode)
        {
            case "WoodTaskComplete":
                Debug.Log("World changes: colors brighter!");
                break;

            case "FenceTaskComplete":
                Debug.Log("World changes: fence fixed!");
                break;

        }
    }
}
