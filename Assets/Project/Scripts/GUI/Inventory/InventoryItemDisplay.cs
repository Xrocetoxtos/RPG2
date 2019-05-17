using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemDisplay : MonoBehaviour
{
    public WorldObject item;

    public TextMeshProUGUI textName;
    public Image sprite;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textUseButton;


    //event onClick leidt naar een nog te bepalen void functie met een WorldObject argument.
    public delegate void InventoryItemDisplayDelegate(WorldObject item);
    public static event InventoryItemDisplayDelegate onClick;

    void Start()
    {
        //items in inventory vanaf start
        if (item != null)
        {
            Prime(item);
        }
    }

    public void Prime(WorldObject item)
    {
        this.item = item;
        if (textName != null)
        {
            textName.SetText(item.objectTitle);
        }
        if (sprite != null)
        {
            sprite.sprite = item.objectSprite;
        }
        if (textDescription != null)
        {
            textDescription.SetText(item.objectDescription);
        }
        if (textUseButton != null)
        {
           /* switch (item.GetComponent<WorldObject>().type)
            {
                case WorldObjectTypes.Armour:
                    {
                        textUseButton.SetText("Equip");
                        break;
                    }
                case WorldObjectTypes.Weapon:
                    {
                        textUseButton.SetText("Wield");
                        break;
                    }
                case WorldObjectTypes.Book:
                    {
                        textUseButton.SetText("Read");
                        break;
                    }
                case WorldObjectTypes.Food:
                    {
                        textUseButton.SetText("Consume");
                        break;
                    }
                default:
                    {
                        textUseButton.SetText("Use");
                        break;
                    }
            }*/
        }
    }

    public void Click()
    {
        Debug.Log("click");
        string displayName = "nothing";
        if (item != null)
        {
            displayName = item.objectTitle;
        }

        Debug.Log("Clicked" + displayName);
        if (onClick != null)
        {
            onClick.Invoke(item);
        }
        else
        {
            Debug.Log("Nobody was listening to " + item.objectTitle);
        }
    }
}