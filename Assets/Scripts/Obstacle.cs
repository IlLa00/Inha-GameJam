using System.ComponentModel;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Obstacle : InteractiveObject
{

    private bool IsHide = false;

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
        if(!IsHide)
        {
            Debug.Log("캐비닛에 숨었다!");
            IsHide = true;
            Player.OnHide();
        }
        else
        {
            Debug.Log("캐비닛에 나왔다!");
            IsHide = false;
            Player.OffHide();
        }
    }
}
