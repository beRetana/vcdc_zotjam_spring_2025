using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public Dictionary<string, bool> itemStates = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        itemStates.Add("Flower", false);
        itemStates.Add("Gem", false);
        itemStates.Add("Cologne", false);
    }

    public void SetItemState(string itemID, bool newState)
    {
        if (itemStates.ContainsKey(itemID))
        {
            itemStates[itemID] = newState;
            Debug.Log($"Item {itemID} set to {newState}");
        }
        else
        {
            Debug.LogWarning($"Item {itemID} not found in dictionary");
        }
    }

    public bool GetItemState(string itemID)
    {
        if (itemStates.TryGetValue(itemID, out bool value))
        {
            return value;
        }

        Debug.LogWarning($"Item {itemID} not found in dictionary");
        return false;
    }
}