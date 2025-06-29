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
    //public event Action<Transform> PlayerTransform; // PlayerController���� ������ ��
    private void Start()
    {
        //player.PlayerTransform += StartChase;
    }

    private void StartChase(Transform playerTransform)
    {
        //killer.ChasePlayer(playerTransform); // KillerAI���� �ش� ��ġ�� �ٷ� �̵��ϴ� �Լ��� ������ ��;
    }
    private void LateUpdate()
    {
        gaugeImage.fillAmount = player.GetCurrentNoiseLevel() / player.GetNoiseLevel();
    }
}
