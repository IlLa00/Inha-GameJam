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

        gameSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!generator.IsPlayGame()) return;

        gameSlider.gameObject.SetActive(true);

        SetSuccessZone();

        // Debug.Log(SuccessZoneMax);
        // Debug.Log(SuccessZoneMin);

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


        if (Input.GetKeyDown(KeyCode.Space))
        {
            float finalValue = gameSlider.value;

            // 성공 구역 내에 있는지 확인
            bool success = IsInSuccessZone(finalValue);

            Debug.Log($"Slider stopped at: {finalValue:F3}, Success: {success}");

            if (success)
            {
                Debug.Log("Success");
                generator.SuccessMiniGame();
            }
            else
                generator.FailMiniGame();


            gameSlider.gameObject.SetActive(false);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) // 이동키 입력 시, 탈출
        {
            isMovingRight = false;
            gameSlider.gameObject.SetActive(false);
        }
    }

    private bool IsInSuccessZone(float value)
    {
        return value >= SuccessZoneMin && value <= SuccessZoneMax;
    }

    private void SetSuccessZone()
    {
        SuccessZoneMin = generator.GetMinRange();
        SuccessZoneMax = generator.GetMaxRange();

        Slider slider = gameSlider.GetComponent<Slider>();
        RectTransform fillAreaRect = slider.fillRect.parent.GetComponent<RectTransform>();

        successZone.SetParent(fillAreaRect, false);

        float normalizedMin = Mathf.Clamp01(SuccessZoneMin);
        float normalizedMax = Mathf.Clamp01(SuccessZoneMax);

        float zoneWidth = (normalizedMax - normalizedMin);
        float zoneStartX = normalizedMin;

        successZone.anchorMin = new Vector2(zoneStartX, 0);
        successZone.anchorMax = new Vector2(normalizedMax, 1);
        successZone.sizeDelta = Vector2.zero;
        successZone.anchoredPosition = Vector2.zero;

        Image zoneImage = successZone.GetComponent<Image>();
        if (zoneImage == null)
            zoneImage = successZone.gameObject.AddComponent<Image>();
        zoneImage.color = Color.red;
    }

}
