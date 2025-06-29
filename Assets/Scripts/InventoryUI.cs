using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Image item;
    [SerializeField]
    private PlayerController player;
    private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        //Inventory Inventory = GetComponent<Inventory>();
        inventory =  player.GetInventory();
        inventory.AddInventoryUI += GetItem;
        inventory.UseInventoryUI += UseItem;
        Color color = item.color;
        color.a = 0;
        item.color = color;
    }

    public void GetItem()
    {
        Color color = item.color;
        color.a = 100;
        item.color = color;
        item.sprite = inventory.getItems()[0].Key.Itemicon;
    }

    public void UseItem()
    {
        Color color = item.color;
        color.a = 0;
        item.color = color;
        item.sprite  = null;
    }
}
