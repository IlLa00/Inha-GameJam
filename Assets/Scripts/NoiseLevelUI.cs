using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseLevelUI : MonoBehaviour
{
    [SerializeField]
    private Image gaugeImage;
    [SerializeField]
    private PlayerController player;

    // Update is called once per frame
    private void Update()
    {
        gaugeImage.fillAmount = player.GetCurrentNoiseLevel() / player.GetNoiseLevel();
    }
}
