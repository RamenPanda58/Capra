using TMPro;
using UnityEngine;

public class HotbarInputReader : MonoBehaviour
{
    GameObject currentLetter;

    void Update()
    {
        for (int i = 1; i <= 7; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                TryReadSlot(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    private void TryReadSlot(int slot)
    {
        LetterData letter = HotbarInventory.Instance.GetLetter(slot);

        if (letter == null)
        {
            Debug.Log("Slot " + slot + " is empty.");
            return;
        }
        Close();

        // Use your existing UI reader
        currentLetter = letter.text;
        Read(letter.text);
    }

    public void Read(GameObject letter)
    {
        letter.SetActive(true);
    }

    public void Close()
    {
        if (currentLetter != null)
            currentLetter.SetActive(false);

    }
}
