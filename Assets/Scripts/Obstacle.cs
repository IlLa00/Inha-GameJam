using UnityEngine;

public class Obstacle : InteractiveObject
{
    void Start()
    {
        
    }

    void Update()
    {
       
    }

    public override void OnInteractive()
    {
        Debug.Log("Starting Obstacle OnInteractive");

        if(IsInteract())
        {
            IsInteracting = false;
            OnHide();
        }
        else
        {
            IsInteracting = true;
            OffHide();
        }

    }

    void OnHide()
    {
        var PlayerController = Player.GetComponent<MonoBehaviour>(); // 플레이어 컨트롤러 비활성화
        if (PlayerController != null)
            PlayerController.enabled = false;

        // 시야 변환?
    }

    void OffHide()
    {
        var PlayerController = Player.GetComponent<MonoBehaviour>(); // 플레이어 컨트롤러 비활성화
        if (PlayerController != null)
            PlayerController.enabled = true;

        // 시야 변환
    }
}
