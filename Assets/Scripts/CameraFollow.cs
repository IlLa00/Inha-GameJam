using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;        // ���� ���
    [SerializeField] private Vector2 minPosition;     // ī�޶� �̵� �ּҰ�
    [SerializeField] private Vector2 maxPosition;     // ī�޶� �̵� �ִ밪

    void Update()
    {
        if (player == null)
            return;

        Vector3 targetPos = player.position;

        // ī�޶� ��ġ ���� (�� ��� �����θ� �̵�)
        float clampedX = Mathf.Clamp(targetPos.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}