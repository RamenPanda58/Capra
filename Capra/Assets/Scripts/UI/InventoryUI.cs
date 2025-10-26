using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // Panel that holds all items
    [SerializeField] private GameObject itemUIPrefab;   // Prefab for one item line

    private Dictionary<string, GameObject> uiItems = new();

    private void Start()
    {
        PlayerInventory.Instance.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void RefreshUI()
    {
        var inventory = PlayerInventory.Instance;
        if (inventory == null) return;

        // Destroy old UI elements
        foreach (Transform child in inventoryPanel.transform)
            Destroy(child.gameObject);

        uiItems.Clear();

        // Add one entry per item
        foreach (var item in inventory.GetAllItems())
        {
            var go = Instantiate(itemUIPrefab, inventoryPanel.transform);
            var texts = go.GetComponentsInChildren<TextMeshProUGUI>();

            if (texts.Length >= 1)
                texts[0].text = $"{item.Key} ×{item.Value}";

            uiItems[item.Key] = go;
        }
    }
}
