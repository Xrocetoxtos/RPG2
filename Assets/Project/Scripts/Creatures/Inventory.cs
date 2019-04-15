using System.Collections.Generic;


public class Inventory
{
    private int creatureCoins = 0;
    private List<WorldObject> creatureInventory = new List<WorldObject>();
    // TODO - een verwijzing naar de display.
    
    //Items
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
        //display bijwerken
    }

    public void RemoveItem(WorldObject item)
    {
        if(GetItem(item) !=null)
        {
            creatureInventory.Remove(item);
            //display bijwerken
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
}
