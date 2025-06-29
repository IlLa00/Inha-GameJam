using UnityEngine;

public class MonsterDropSystem : MonoBehaviour
{
    [System.Serializable]
    public class DropData
    {
        public GameObject itemPrefab;
        [Range(0f, 1f)]
        public float dropChance = 1f;
    }

    [SerializeField] private DropData[] dropItems;

    public void DropItems()
    {
        foreach (DropData dropData in dropItems)
        {
            if (Random.Range(0f, 1f) <= dropData.dropChance)
            {
                GameObject droppedItem = Instantiate(dropData.itemPrefab, transform.position, Quaternion.identity);

                // 드롭된 아이템으로 설정
                Item itemComponent = droppedItem.GetComponent<Item>();
                if (itemComponent != null)
                {
                    itemComponent.SetAsDroppedItem();
                }
            }
        }
    }
}
