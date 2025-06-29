using UnityEngine;
using UnityEngine.UI;

public class InteractiveObjectUI : MonoBehaviour
{
    [SerializeField] private InteractiveObject interactiveobject;
    [SerializeField] private GameObject InteractiveUI;

    void Start()
    {
        if (interactiveobject == null) return;
        if (InteractiveUI == null) return;

        Vector3 generatorWorldPosition = transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(generatorWorldPosition);

        RectTransform uiRectTransform = InteractiveUI.GetComponent<RectTransform>();
        uiRectTransform.position = screenPosition + new Vector3(0, 50, 0); // 스크린 상에서 Y축으로 50픽셀 위로
    }

    void Update()
    {
        if (interactiveobject.CanInteract())
            InteractiveUI.SetActive(true);
        else
            InteractiveUI.SetActive(false);
    }
}
