using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    bool isSetActive = false;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(isSetActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSetActive = isSetActive ? false : true;
            panel.SetActive(isSetActive);        
        }

    }

}
