using UnityEngine;
using UnityEngine.UI;

public class DroneCameraUI : MonoBehaviour
{
    [SerializeField] private Camera _photoCamera;
    [SerializeField] private Image _imageTargetSelector;

    public void SetSelectorPosition(Vector3? position)
    {
        Vector2 screenPos;
        if (position != null)
            screenPos = _photoCamera.WorldToScreenPoint((Vector3)position);
        else
            screenPos = new Vector2(_photoCamera.pixelWidth / 2, _photoCamera.pixelHeight / 2);

        RectTransform rect = _imageTargetSelector.rectTransform;
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, screenPos, Time.deltaTime * 10f);
    }
}