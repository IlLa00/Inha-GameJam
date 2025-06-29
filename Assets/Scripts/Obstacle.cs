using UnityEngine;

public class Obstacle : InteractiveObject
{
    private bool isHide = false;

    void Start()
    {
        
    }

    void Update()
    {
        base.Update();
    }

    public override void OnInteractive()
    {
        Debug.Log("Starting Obstacle OnInteractive");

        OnOffHide();
    }

    void OnOffHide()
    {
        if(!isHide)
        {
            Debug.Log("캐비닛에 숨었다!");
            isHide = true;

            Player.OnHide();
        }
        else
        {
            Debug.Log("캐비닛에 나왔다!");
            isHide = false;

            Player.OffHide();
        }
    }
}
