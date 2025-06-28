using UnityEngine;

public class Obstacle : InteractiveObject
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void OnInteractive()
    {
        // 부모에서 플레이어 널값 검사
        Debug.Log("Starting Obstacle OnInteractive");

        //var PlayerController = player.GetComponent<MonoBehaviour>(); // 플레이어 컨트롤러 비활성화
        //if (PlayerController != null)
        //    PlayerController.enabled = false;

        // 시야 변환

    }
}
