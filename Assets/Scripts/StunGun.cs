using UnityEngine;
using UnityEngine.UI;

public class StunGun : Item
{
    private Camera playerCamera;
    private float range = 10f;                    
    private LayerMask targetLayer = -1;

    void Awake()
    {
        Initialize("Stun Gun", Resources.Load<Sprite>("StunGun"));

    }

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Itemicon;
        renderer.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        playerCamera = Camera.main;
        if (playerCamera == null)
            playerCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        
    }

    public override void OnExecute()
    {
        FireStunGun();
    }

    private void FireStunGun()
    {
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        RaycastHit hit;

        // 레이캐스트 실행
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, range, targetLayer))
        {
            Debug.Log($"스턴건이 {hit.collider.name}에 적중!");

            // 적중한 오브젝트 처리
            //HandleStunHit(hit);

            // 적중 위치에 이펙트 생성
            
        }
        else
        {
            Debug.Log("스턴건이 빗나갔습니다.");

            // 빗나간 경우 최대 사거리 지점에 이펙트
            Vector3 endPoint = rayOrigin + rayDirection * range;
            
        }

        Debug.DrawRay(rayOrigin, rayDirection * range, Color.yellow, 1f);
    }
}

