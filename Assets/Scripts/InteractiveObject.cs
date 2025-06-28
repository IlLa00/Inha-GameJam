using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected float interactionRange = 3f;
    // [SerializeField] protected Player player;

    protected bool CanInteract; 

    void Start()
    {
        
    }

    void Update()
    {
        //float distance = Vector3.Distance(transform.position, player.position);
        //if (distance < interactionRange)
        //    CanInteract = true;
        //else
        //    CanInteract = false;


    }

    protected virtual void OnInteractive() {}
    protected virtual void OffInteractive() { }

}
