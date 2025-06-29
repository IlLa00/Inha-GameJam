using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Sprite StunGunSprite;
    [SerializeField]
    private Image item;
    // Start is called before the first frame update
    void Start()
    {
        Color color = item.color;
        color.a = 0;
        item.color = color;
    }

    public void GetItem()
    {
        Color color = item.color;
        color.a = 100;
        item.color = color;
        item.sprite = StunGunSprite;
    }

    public void UseItem()
    {
        Color color = item.color;
        color.a = 0;
        item.color = color;
        item.sprite  = null;
    }
}
