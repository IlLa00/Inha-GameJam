using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected float interactionRange = 0.6f;
    [SerializeField] protected PlayerController Player;

    protected bool CanInteracting;
    protected bool IsInteracting;
    protected float distance;
    
    public bool CanInteract() { return CanInteracting; }
    public bool IsInteract() { return IsInteracting; }

    void Start()
    {

    }

    protected virtual void Update()
    {
        distance = Vector3.Distance(Player.transform.position, transform.position);

        if (distance < interactionRange)
            CanInteracting = true;
        else
            CanInteracting = false;

        if(IsInteract() && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
            OffInteractive();
        
    }

    public virtual void OnInteractive()
    {
        if (!IsInteract())
            IsInteracting = true;
    }

    protected virtual void OffInteractive()
    {
        if (IsInteract())
            IsInteracting = false;
    }

}