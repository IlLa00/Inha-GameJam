using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<KeyValuePair<Item, int>> items = new List<KeyValuePair<Item, int>>();
    int CurrentItemIndex = 0;

    public bool IsInventoryEmpty() { return items.Count <= 0; }

    public event Action AddInventoryUI;
    public event Action UseInventoryUI;

    [SerializeField] public PlayerController Owner;

    void Start()
    {
        if (Owner == null)
            return;
    }

    void Update()
    {
        
    }

    public List<KeyValuePair<Item, int>> getItems()
    {
        return items;
    }
    public void AddItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Key == item)
            {
                // 기존 아이템의 수량 증가
                items[i] = new KeyValuePair<Item, int>(item, items[i].Value + 1);
                Debug.Log($"{item.name} 추가됨. 현재 수량: {items[i].Value}");
                return;
            }
        }
        item.Owner = Owner;
        // 새로운 아이템 추가
        items.Add(new KeyValuePair<Item, int>(item, 1));
        AddInventoryUI?.Invoke();
        Debug.Log($"{item.name} 새로 추가됨. 수량: 1");
    }

    public void UseItem()
    {
        if (CurrentItemIndex < 0 || CurrentItemIndex >= items.Count)
            return;

        KeyValuePair<Item, int> currentItem = items[CurrentItemIndex];

        if (currentItem.Value <= 0)
            return;

        Debug.Log($"{currentItem.Key} 사용!");

        items[CurrentItemIndex].Key.OnExecute(); // 아이템 사용!!

        int newQuantity = currentItem.Value - 1;
        if (newQuantity <= 0)
        {
            UseInventoryUI?.Invoke();
            items.RemoveAt(CurrentItemIndex);
            Debug.Log($"{currentItem.Key}이(가) 모두 소모되어 인벤토리에서 제거되었습니다.");
        }
        else
        {
            items[CurrentItemIndex] = new KeyValuePair<Item, int>(currentItem.Key, newQuantity);
            Debug.Log($"{currentItem.Key} 사용 완료. 남은 수량: {newQuantity}");
        }


    }
}
