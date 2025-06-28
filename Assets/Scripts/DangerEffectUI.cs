using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerEffectUI : MonoBehaviour
{
    [SerializeField]
    private Transform PlayerTransform;
    [SerializeField]
    private Transform KillerTransform;
    [SerializeField]
    private Image dangerImage;
    private Color dangerColor = new Color(1f, 0f, 0f, 0f);

    float maxDistance = 20.0f;
    float minDistance = 5.0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(PlayerTransform.position, KillerTransform.position);
        if (distance < maxDistance)
        {
            float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
            dangerColor.a = Mathf.Lerp(0f, 0.8f, 1f - t);
            dangerImage.color = dangerColor;
        }
        else
        {
            dangerColor.a = 0f;
            dangerImage.color = dangerColor;
        }
    }
}
