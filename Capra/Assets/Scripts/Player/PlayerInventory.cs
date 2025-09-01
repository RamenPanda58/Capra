using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(string itemName, int amount = 1)
    {
        if (items.ContainsKey(itemName))
            items[itemName] += amount;
        else
            items[itemName] = amount;
    }

    public bool RemoveItem(string itemName, int amount = 1)
    {
        if (!items.ContainsKey(itemName)) return false;

        items[itemName] -= amount;

        if (items[itemName] <= 0)
            items.Remove(itemName);

        return true;
    }

    public bool HasItem(string itemName, int amount = 1)
    {
        return items.ContainsKey(itemName) && items[itemName] >= amount;
    }

    public int GetItemCount(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }

    public string GetHeldItem()
    {
        foreach (var kvp in items)
            return kvp.Key;

        return null;
    }
}
