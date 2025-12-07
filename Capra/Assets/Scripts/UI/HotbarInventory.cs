using UnityEngine;

public class HotbarInventory : MonoBehaviour
{
    public static HotbarInventory Instance;

    public LetterData[] slots;

    [SerializeField] private int hotbarSize = 7;

    private void Awake()
    {
        Instance = this;
        slots = new LetterData[hotbarSize];
    }

    public bool AddLetter(LetterData letter)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = letter;
                Debug.Log("Stored letter in slot " + (i + 1));
                return true;
            }
        }

        Debug.Log("Hotbar full!");
        return false;
    }

    public LetterData GetLetter(int slot)
    {
        if (slot < 1 || slot > slots.Length)
            return null;

        return slots[slot - 1];
    }
}
