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
            sprite.sprite = item.objectSprite;
        }
        if (buttonUse!=null)
        {
            buttonUse.SetText(item.useButton);
        }
        if (buttonDrop != null)
        {
            buttonDrop.SetText(item.dropButton);
        }
    }
}
