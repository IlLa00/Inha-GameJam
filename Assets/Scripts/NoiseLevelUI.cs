using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseLevelUI : MonoBehaviour
{
    [SerializeField]
    private Image gaugeImage;
    [SerializeField]
    private PlayerController player; // 
    [SerializeField]
    private KliierAI killer;
    // // PlayerController에서 설정할 것

    private void Start()
    {
        player.GetNoisePos += StartChase;
    }

    private void StartChase(Transform NoisePos)
    {
        Debug.Log("!!!");
        killer.ChasePlayer(NoisePos); // KillerAI에서 해당 위치로 바로 이동하는 함수를 만들어야 함;
    }
    private void LateUpdate()
    {
        gaugeImage.fillAmount = player.GetCurrentNoiseLevel() / player.GetNoiseLevel();
    }
}
