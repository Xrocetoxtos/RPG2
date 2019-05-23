using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public Transform panel;
    public Transform targetTransform;
    public InventoryItemDisplay itemDisplayPrefab;
    public InventoryItemDisplay selectedItemDisplay;
    private bool selectedPrimed = false;
    [SerializeField] private Inventory inventory;
    private WorldObject worldObject;
    [SerializeField]private List<InventoryItemDisplay> listOfItems= new List<InventoryItemDisplay>();

    private void Awake()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        panel = GameObject.Find("ListPanel").transform;
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
        if (selectedItemDisplay.item==null)
        {
            selectedItemDisplay.Empty();
        }
        selectedPrimed = false;
    }

    public void PrimeItem(WorldObject item)
    {
        InventoryItemDisplay display = (InventoryItemDisplay)Instantiate(itemDisplayPrefab);
        display.transform.SetParent(targetTransform, false);
        display.Prime(item);
        worldObject = item;
        listOfItems.Add(display);
    }

    public void SelectObject(WorldObject item)
    {
        selectedItemDisplay.Prime(item);
        selectedPrimed = true;
    }

    public void DropItem()
    {
        bool found = false;     // het betreffende item moet weggegooid
        bool selected = false;  // het eerste niet-betreffende item moet geselecteerd
        WorldObject selItem = null;
        foreach (Transform child in panel)
        {
            if (!selected || !found)
            { 
                if (child.gameObject.GetComponent<InventoryItemDisplay>().item == selectedItemDisplay.item)
                {
                    selectedItemDisplay.item.DropItem();
                    Destroy(child.gameObject);
                    found = true;
                }
                else if(!selected)
                {
                    selItem = child.gameObject.GetComponent<InventoryItemDisplay>().item;
                    selected = true;
                }
            }   
        }

        if (!selected)
        {
            selectedItemDisplay.item = null;
            selectedItemDisplay.Empty();
        }
        else
        {
            SelectObject(selItem);
        }
    }
}
