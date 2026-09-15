using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<Item> items = new List<Item>();

    [Header("Inventory UI")]
    public Image[] inventorySlots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RefreshInventoryUI();
    }

    public void AddItem(Item item)
    {
        items.Add(item);

        Debug.Log(item.itemName + " Added!");

        RefreshInventoryUI();
    }

    private void RefreshInventoryUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < items.Count)
            {
                // Show item
                inventorySlots[i].sprite = items[i].icon;
                inventorySlots[i].preserveAspect = true;
                inventorySlots[i].enabled = true;
            }
            else
            {
                // Empty slot = hide ItemIcon
                inventorySlots[i].sprite = null;
                inventorySlots[i].enabled = false;
            }
        }
    }

    public bool HasItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
                return true;
        }

        return false;
    }

    public bool RemoveItem(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == itemName)
            {
                Debug.Log(itemName + " Removed!");

                items.RemoveAt(i);

                RefreshInventoryUI();

                return true;
            }
        }

        return false;
    }
}