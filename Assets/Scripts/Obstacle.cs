using UnityEngine;

public class Obstacle : InteractiveObject
{
    public bool IsHide = false;

    void Start()
    {
        
    }

    void Update()
    {
        if(IsHide)
        {

        }
    }

    protected override void OnInteractive()
    {
        Debug.Log("Starting Obstacle OnInteractive");

        var PlayerController = Player.GetComponent<MonoBehaviour>(); // 플레이어 컨트롤러 비활성화
        if (PlayerController != null)
            PlayerController.enabled = false;

        // 시야 변환

    }
}
