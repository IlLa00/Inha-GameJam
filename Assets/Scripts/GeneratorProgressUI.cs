using UnityEngine;
using UnityEngine.UI;

public class GeneratorProgressUI : MonoBehaviour
{
    [SerializeField] private Slider gameSlider;
    [SerializeField] private Generator generator;

    void Start()
    {
        if (gameSlider == null) return;
        if (generator == null) return;

        gameSlider.minValue = 0f;
        gameSlider.maxValue = 1f;
        gameSlider.value = 0f;
        gameSlider.interactable = false;

        Vector3 generatorWorldPosition = transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(generatorWorldPosition);

        RectTransform uiRectTransform = gameSlider.GetComponent<RectTransform>();
        uiRectTransform.position = screenPosition + new Vector3(0, 50, 0); // 스크린 상에서 Y축으로 50픽셀 위로
    }

    void Update()
    {
        if (generator.IsRepair())
        {
            gameSlider.value = generator.GetCurrentProgress();
        }
    }
}
