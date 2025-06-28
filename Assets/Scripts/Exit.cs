using UnityEngine;

public class Exit : InteractiveObject
{
    public bool CanEscape = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void OnInteractive()
    {
        Debug.Log("Starting Exit OnInteractive");
        
        // 발전기가 다 켜져있는지. 안켜져있으면 얼리 리턴

    }
}
