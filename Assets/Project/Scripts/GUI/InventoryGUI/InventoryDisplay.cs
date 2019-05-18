using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public Transform targetTransform;
    public InventoryItemDisplay itemDisplayPrefab;
    public InventoryItemDisplay selectedItemDisplay;
    private bool selectedPrimed = false;
    [SerializeField] private Inventory inventory;
    private WorldObject worldObject;

    private void Awake()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
    }

    public void Prime(List<WorldObject> items)
    {
        foreach (WorldObject item in items)
        {
            if (selectedPrimed==false)
            {
                SelectObject(item);
            }
            PrimeItem(item);
        }
        selectedPrimed = false;
    }

    public void PrimeItem(WorldObject item)
    {
        InventoryItemDisplay display = (InventoryItemDisplay)Instantiate(itemDisplayPrefab);
        display.transform.SetParent(targetTransform, false);
        display.Prime(item);
        worldObject = item;
    }

    public void SelectObject(WorldObject item)
    {
        selectedItemDisplay.Prime(item);
        selectedPrimed = true;
    }

    public void DropItem()
    {
        selectedItemDisplay.item.DropItem();

        //opnieuw opbouwen
    }
}
