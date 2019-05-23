using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour
{
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDescription;
    public Image sprite;
    public TextMeshProUGUI buttonUse;
    public TextMeshProUGUI buttonDrop;

    //de objecten zelf
    public GameObject useButton;
    public GameObject dropButton;

    public WorldObject item;

    public void Prime(WorldObject item)
    {
        this.item = item;
        if(textName!=null)
        {
            textName.SetText(item.objectTitle);
        }
        if (textDescription != null)
        {
            textDescription.SetText(item.objectDescription);
        }
        if (sprite != null)
        {
            sprite.gameObject.SetActive(true);
            sprite.sprite = item.objectSprite;
        }
        Button1();
        Button2();
    }
    
    public void Empty()
    {
        if (textName != null)
        {
            textName.SetText("");
        }
        if (textDescription != null)
        {
            textDescription.SetText("Your pockets are empty");
        }
        if (sprite != null)
        {
            sprite.gameObject.SetActive(false);
        }
        if (buttonUse != null)
        {
            useButton.SetActive(false);
            buttonUse.SetText("");
        }
        if (buttonDrop != null)
        {
            dropButton.SetActive(false);
            buttonDrop.SetText("");
        }
    }

    private void Button1()
    {
        if (buttonUse == null || useButton==null) return;

        useButton.SetActive(true);
        if (item.useButton != "")
        {
            buttonUse.SetText(item.useButton);
            return;
        }
        if(item.foodObject)
        {
            buttonUse.SetText("Consume");
            return;

        }
        if (item.weaponObject || item.attireObject)
        {
            buttonUse.SetText("Equip");
            return;
        }
        buttonUse.SetText("Use");
    }

    private void Button2()
    {
        if (buttonDrop == null || dropButton==null) return;

        dropButton.SetActive(true);
        if (item.dropButton != "")
        {
            buttonDrop.SetText(item.dropButton);
            return;
        }
        buttonDrop.SetText("Drop");
    }

}
