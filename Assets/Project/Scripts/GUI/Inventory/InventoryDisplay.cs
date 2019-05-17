using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryDisplay : MonoBehaviour
{
    public Transform targetTransform;
    public InventoryItemDisplay itemDisplayPrefab;
    public InventoryItemDisplay selectedItemDisplay;
    public bool selectedPrimed = false;

    public Inventory inventory;

    public void Prime(Inventory inventory)
    {
        this.inventory = inventory;
        List<WorldObject> items = inventory.creatureInventory;
        selectedItemDisplay.item = null;
        foreach (WorldObject item in items)
        {
            PrimeItem(item);
        }
    }

    public void PrimeItem(WorldObject item)
    {
        InventoryItemDisplay display = (InventoryItemDisplay)Instantiate(itemDisplayPrefab);
        display.transform.SetParent(targetTransform, false);
        display.Prime(item);
        if (!selectedPrimed)
        {
            selectedItemDisplay.item = item;
            selectedItemDisplay.textName.SetText(item.objectTitle);
            selectedItemDisplay.textDescription.SetText(item.objectDescription);
            selectedItemDisplay.sprite.sprite = item.objectSprite;
            selectedPrimed = true;
        }
    }
}