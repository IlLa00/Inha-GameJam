using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;
    [SerializeField]
    private GameObject GameOverPanel;
    [SerializeField]
    private PlayerController player;
    bool isSetActive = false;
    // Start is called before the first frame update
    void Start()
    {
        GameOverPanel.SetActive(false);
        player.OpenGameOverPanel += OpenGameOverPanel;
        optionPanel.SetActive(isSetActive);
    }

    void OpenGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSetActive = isSetActive ? false : true;
            optionPanel.SetActive(isSetActive);        
        }

    }

}
