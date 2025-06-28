using UnityEngine;
using UnityEngine.UI;

public class GeneratorGameUI : MonoBehaviour
{
    [SerializeField] private Slider gameSlider;
    [SerializeField] private RectTransform successZone;
    [SerializeField] private Generator generator;

    [SerializeField] private float sliderSpeed = 1f;

    private float SuccessZoneMin;
    private float SuccessZoneMax;

    private bool isMovingRight = true;

    void Start()
    {
        if (gameSlider == null) return;
        if (generator == null) return;

        gameSlider.minValue = 0f;
        gameSlider.maxValue = 1f;
        gameSlider.value = 0f;
        gameSlider.interactable = false;

        SuccessZoneMin = generator.GetMinRange();
        SuccessZoneMax = generator.GetMaxRange();

        RectTransform sliderRect = gameSlider.GetComponent<RectTransform>();
        successZone.SetParent(sliderRect, false);

        // 성공 구역 위치 및 크기 계산
        float zoneWidth = (SuccessZoneMax - SuccessZoneMin) * sliderRect.rect.width;
        float zoneStartX = SuccessZoneMin * sliderRect.rect.width - sliderRect.rect.width * 0.5f;

        // 성공 구역 설정
        successZone.anchorMin = new Vector2(0, 0);
        successZone.anchorMax = new Vector2(0, 1);
        successZone.sizeDelta = new Vector2(zoneWidth, 0);
        successZone.anchoredPosition = new Vector2(zoneStartX + zoneWidth * 0.5f, 0);

        // 빨간색으로 설정
        Image zoneImage = successZone.GetComponent<Image>();
        if (zoneImage == null)
            zoneImage = successZone.gameObject.AddComponent<Image>();

        zoneImage.color = Color.red;

        gameSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!generator.IsPlayGame()) return;

        gameSlider.gameObject.SetActive(true);

        float CurrentValue = gameSlider.value;

        if (isMovingRight)
        {
            CurrentValue += sliderSpeed * Time.deltaTime;
            if (CurrentValue >= 1f)
            {
                CurrentValue = 1f;
                isMovingRight = false;
            }
        }
        else
        {
            CurrentValue -= sliderSpeed * Time.deltaTime;
            if (CurrentValue <= 0f)
            {
                CurrentValue = 0f;
                isMovingRight = true;
            }
        }

        gameSlider.value = CurrentValue;


        if(Input.GetKeyDown(KeyCode.Space))
        {
            float finalValue = gameSlider.value;

            // 성공 구역 내에 있는지 확인
            bool success = IsInSuccessZone(finalValue);

            Debug.Log($"Slider stopped at: {finalValue:F3}, Success: {success}");

            if (success)
                generator.SuccessMiniGame();
            else
                generator.FailMiniGame();


            gameSlider.gameObject.SetActive(false);
        }
    }

    private bool IsInSuccessZone(float value)
    {
        return value >= SuccessZoneMin && value <= SuccessZoneMax;
    }
       
}
