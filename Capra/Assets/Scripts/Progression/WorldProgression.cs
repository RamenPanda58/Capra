using UnityEngine;

public class WorldProgression : MonoBehaviour
{
    public static WorldProgression Instance { get; private set; }

    public GameObject wizard1;
    public GameObject wizard2;

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
            case "WoodCuttingFinished":
                wizard1.SetActive(false);
                wizard2.SetActive(true);
                break;

            case "Coin":
                Debug.Log("World changes: fence fixed!");
                break;

        }
    }
}
