using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private string Itemname;
    private Sprite Itemicon;

    public Item(string name)
    {
        Itemname = name;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected abstract void OnExecute();
}
