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

            MonsterAI m = monster.GetComponent<MonsterAI>();
            m.OnDeath += () =>
            {
                StartCoroutine(RespawnAfterDelay(data, monster));
            };
        }
    }

    private IEnumerator RespawnAfterDelay(MonsterSpawnData data, GameObject monsterObj)
    {
        yield return new WaitForSeconds(data.respawnDelay);

        // 다시 위치 초기화 및 활성화
        monsterObj.transform.position = data.spawnPosition;
        monsterObj.SetActive(true);

        monsterObj.GetComponent<MonsterAI>()?.ResetMonster();
        //var collider = monsterObj.GetComponent<Collider2D>();
        //if (collider != null) collider.enabled = true;

        //var monsterAI = monsterObj.GetComponent<MonsterAI>();
        //if (monsterAI != null) monsterAI.enabled = true;
        //monsterAI.HP = 1;
        //monsterAI.enabled = true;

        //monsterAI.ChangeState(0);
        //var animator = monsterObj.GetComponent<Animator>();
        //if (animator != null) animator.Rebind();
    }
    
}
