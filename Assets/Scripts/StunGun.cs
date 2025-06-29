using UnityEngine;
using UnityEngine.UI;

public class StunGun : Item
{
    private Camera playerCamera;
    private float range = 100f;
    private float key = 1;

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
        Vector2 rayOrigin = Owner.gameObject.transform.position;
        Vector2 rayDirection = new Vector2(Owner.gameObject.transform.localScale.x > 0 ? key : -key, 0);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, range, LayerMask.GetMask("Killer"));
        if (hit.collider != null)
        {
            Debug.Log($"Ray가 {hit.collider.name}을 맞춤, 태그: {hit.collider.tag}");

            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log($"스턴건이 적중: {hit.collider.name}");
                KliierAI killer = hit.collider.GetComponent<KliierAI>();
                if (killer)
                {
                    killer.TakeDamage(1);
                }
            }
            else
            {
                Debug.Log("맞긴 했지만 Enemy 태그가 아님");
            }
        }
        else
        {
            Debug.Log("스턴건이 아무것도 못 맞춤");
        }
    }
}

