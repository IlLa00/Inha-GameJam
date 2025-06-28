using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected float interactionRange = 0.6f;
    [SerializeField] protected PlayerController Player;

    protected bool CanInteracting;
    protected float distance;
    
    public bool CanInteract() { return CanInteracting; }

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
        
    }

    public virtual void OnInteractive() { }
    protected virtual void OffInteractive() { }

}