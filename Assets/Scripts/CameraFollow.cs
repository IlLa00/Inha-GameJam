using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;        // 따라갈 대상
    [SerializeField] private Vector2 minPosition;     // 카메라 이동 최소값
    [SerializeField] private Vector2 maxPosition;     // 카메라 이동 최대값

    void Update()
    {
        if (player == null)
            return;

        Vector3 targetPos = player.position;

        // 카메라 위치 제한 (맵 경계 안으로만 이동)
        float clampedX = Mathf.Clamp(targetPos.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}