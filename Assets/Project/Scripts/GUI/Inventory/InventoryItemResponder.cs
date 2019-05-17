using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemResponder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InventoryItemDisplay.onClick += InventoryItemDisplay_onClick;
    }

    private void OnDestroy()
    {
        Debug.Log("Unsigned for on-click");
        InventoryItemDisplay.onClick -= InventoryItemDisplay_onClick;
    }

    private void InventoryItemDisplay_onClick(WorldObject item)
    {
        Debug.Log("listening to " + item.objectTitle);
        //throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {

    }
}