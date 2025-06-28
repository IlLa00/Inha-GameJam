using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSpawnData
{
    public GameObject prefab;
    public Vector3 spawnPosition;
    public float respawnDelay;
}

public class MonsterSpawnCenter : MonoBehaviour
{
    [SerializeField]
    private List<MonsterSpawnData> spawnList;

    private List<GameObject> spawnedMonsters = new List<GameObject>();

    private void Start()
    {
        foreach (var data in spawnList)
        {
            GameObject monster = Instantiate(data.prefab, data.spawnPosition, Quaternion.identity);
            spawnedMonsters.Add(monster);

            //Monster m = monster.GetComponent<Monster>();
            //m.OnDeath += (dead) =>
            //{
            //    StartCoroutine(RespawnAfterDelay(data, monster));
            //};
        }
    }

    private IEnumerator RespawnAfterDelay(MonsterSpawnData data, GameObject monsterObj)
    {
        yield return new WaitForSeconds(data.respawnDelay);

        // 다시 위치 초기화 및 활성화
        monsterObj.transform.position = data.spawnPosition;
        monsterObj.SetActive(true);
    }
    
}
