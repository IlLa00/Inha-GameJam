using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer = -1;
    private Item item;

    void Start()
    {
        item = GetComponent<Item>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            // 플레이어 인벤토리에 추가
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddItem(item);
                item.OnPickup(other.gameObject);
            }
        }
    }
}
