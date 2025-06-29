using UnityEngine;
using System.Collections; 

public abstract class Item : MonoBehaviour
{
    private Sprite Itemicon;
    private string Itemname;
    protected bool isDropped = false;

    public virtual void Initialize(string name, Sprite icon)
    {
        Itemname = name;
        Itemicon = icon;
    }

    void Start()
    {
        if(isDropped)
        {
            SetupAsDroppedItem();
        }
    }

    void Update()
    {
        if (isDropped)
        {
            DropItemBehavior();
        }
    }

    public virtual void SetAsDroppedItem()
    {
        isDropped = true;
        SetupAsDroppedItem();
    }

    protected virtual void SetupAsDroppedItem()
    {
        // 시각적 설정
        SetupVisuals();

        // 물리 설정
        SetupPhysics();

        // 픽업 감지 설정
        SetupPickupDetection();

        // 드롭 애니메이션 시작
        StartCoroutine(DropAnimation());
    }

    protected virtual void SetupVisuals()
    {
        // SpriteRenderer 설정
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = gameObject.AddComponent<SpriteRenderer>();

        sr.sprite = Itemicon;
        sr.sortingOrder = 10;

        // 스케일 설정 (필요에 따라)
        transform.localScale = Vector3.one * 0.8f;
    }

    protected virtual void SetupPhysics()
    {
        // Rigidbody2D 추가 (드롭 효과용)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0.5f;
        rb.drag = 2f;

        // 랜덤한 힘 적용
        Vector2 randomForce = new Vector2(Random.Range(-2f, 2f), Random.Range(1f, 3f));
        rb.AddForce(randomForce, ForceMode2D.Impulse);
    }

    protected virtual void SetupPickupDetection()
    {
        // 픽업용 콜라이더
        CircleCollider2D pickupCollider = gameObject.AddComponent<CircleCollider2D>();
        pickupCollider.radius = 1f;
        pickupCollider.isTrigger = true;

        // 픽업 감지 컴포넌트 추가
        if (GetComponent<ItemPickup>() == null)
            gameObject.AddComponent<ItemPickup>();
    }

    protected virtual void DropItemBehavior()
    {
        // 둥둥 떠다니는 효과
        BobEffect();

        // 회전 효과
        transform.Rotate(0, 0, 30f * Time.deltaTime);
    }

    protected virtual void BobEffect()
    {
        float bobAmount = Mathf.Sin(Time.time * 2f) * 0.1f;
        Vector3 pos = transform.position;
        pos.y += bobAmount * Time.deltaTime;
        transform.position = pos;
    }

    protected virtual IEnumerator DropAnimation()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = startScale * 1.2f;

        // 확대 효과
        float time = 0f;
        while (time < 0.2f)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / 0.2f);
            yield return null;
        }

        // 원래 크기로
        time = 0f;
        while (time < 0.2f)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale, startScale, time / 0.2f);
            yield return null;
        }
    }

    public virtual void OnPickup(GameObject picker)
    {
        Debug.Log($"{Itemname} picked up by {picker.name}");

        // 픽업 효과
        StartCoroutine(PickupEffect());
    }

    protected virtual IEnumerator PickupEffect()
    {
        // 위로 올라가면서 사라지는 효과
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * 2f;
        Color startColor = GetComponent<SpriteRenderer>().color;

        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = time / duration;

            // 위로 이동
            transform.position = Vector3.Lerp(startPos, endPos, progress);

            // 투명해짐
            Color color = startColor;
            color.a = Mathf.Lerp(1f, 0f, progress);
            GetComponent<SpriteRenderer>().color = color;

            // 크기 축소
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, progress);

            yield return null;
        }

        Destroy(gameObject);
    }

    public abstract void OnExecute();
}
