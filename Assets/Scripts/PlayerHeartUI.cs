using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeartUI : MonoBehaviour
{
    [SerializeField]
    private Sprite empty_Heart;
    [SerializeField]
    private Sprite half_Heart;
    [SerializeField]
    private Sprite full_Heart;

    [SerializeField]
    private GameObject HeartPrefab;
    [SerializeField]
    private Transform HeartContainer;

    [SerializeField]
    private PlayerController player;
    
    private List<Image> HeartImages = new List<Image>();

    //private void Awake()
    //{
    //    player.UpdateHP += UpdateHP_UI;
    //}

    private void Awake()
    {
        player.UpdateHP += UpdateHP_UI;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(player.HP);
        int HeartCount = Mathf.CeilToInt(player.HP * 0.5f);
        Debug.Log(HeartCount);
        for (int i = 0; i < HeartCount; i++)
        {
            GameObject heart = Instantiate(HeartPrefab, HeartContainer.transform);
            HeartImages.Add(heart.GetComponent<Image>());
        }
    }

    public void UpdateHP_UI(int playerHP)
    {
        int currentHP = playerHP;
        for (int i = 0; i < HeartImages.Count; i++)
        {
            if(currentHP >= 2)
            {
                HeartImages[i].sprite = full_Heart;
                currentHP -= 2;
            }
            else if (currentHP == 1)
            {
                HeartImages[i].sprite = half_Heart;
                currentHP -= 1;
            }
            else
            {
                HeartImages[i].sprite = empty_Heart;
            }
        }
    }
}
