using System.Collections.Generic;
using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int creatureCoins = 0;
    public List<WorldObject> creatureInventory = new List<WorldObject>();

    private Journal journal;
    public bool isPlayer = false;
    // TODO - een verwijzing naar de display.

    private void Awake()
    {
        journal = GetComponent<Journal>();
        if (journal !=null)
        {
            isPlayer = true;
        }
    }

    //Items
    public string AllItems()
    {
        string itemsInInventory = "*";
        foreach(WorldObject item in creatureInventory)
        {
            itemsInInventory += item.objectTitle + " - ";
        }
        return itemsInInventory;
    }

    public WorldObject GetItem(WorldObject item)
    {
        if(creatureInventory.Contains(item))
        {
            return item;
        }
        return null;
    }

    public void AddItem(WorldObject item)
    {
        creatureInventory.Add(item);
        if (isPlayer)
        {
            journal.CheckAllActiveObjectives();
            //display bijwerken
        }
    }

    public void RemoveItem(WorldObject item)
    {
        if(GetItem(item) !=null)
        {
            creatureInventory.Remove(item);
            if (isPlayer)
            {
                journal.CheckAllActiveObjectives();
                //display bijwerken
            }
        }
    }

    // Coins
    public int GetCoins()
    {
        return creatureCoins;
    }

    public void AddCoins(int amount)
    {
        creatureCoins += amount;
    }

    public void RemoveCoins(int amount)
    {
        creatureCoins -= amount;
        if (creatureCoins < 0) creatureCoins = 0;
    }

    public bool EnoughCoins(int amount)
    {
        if(GetCoins() >= amount)
        {
            return true;
        }
        return false;
    }

    //Trade
    //inventory die je hier passt, dat is de inventory van degene waar je mee handelt.
    public void GiveItem(Inventory inventory, WorldObject item)
    {
        if(GetItem(item)!=null)
        {
            inventory.AddItem(item);
            RemoveItem(item);
        }
    }

    public void GiveCoins(Inventory inventory, int amount)
    {
        if(EnoughCoins(amount))
        {
            inventory.AddCoins(amount);
            RemoveCoins(amount);
        }
    }

    public void BuyObject(Inventory inventory, WorldObject item, int amount)
    {
        if (inventory.GetItem(item) != null && EnoughCoins(amount))
        {
            AddItem(item);
            RemoveCoins(amount);
        }
    }

    public void SellObject(Inventory inventory, WorldObject item, int amount)
    {
        if (GetItem(item) != null && inventory.EnoughCoins(amount))
        {
            RemoveItem(item);
            AddCoins(amount);
        }
    }

    public bool EnoughInventory(int amount, WorldObject[] worldObjects)
    {
        foreach (WorldObject item in worldObjects)
        {
            if (GetItem(item)==null)
            {
                return false;
            }
        }
        if (!EnoughCoins(amount))
        {
            return false;
        }
        return true;
    }
}
