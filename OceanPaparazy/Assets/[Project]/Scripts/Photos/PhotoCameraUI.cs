using UnityEngine;
using UnityEngine.UI;

public class PhotoCameraUI : MonoBehaviour
{
    [SerializeField] private PhotoTargetDetector _photoTargetDetector;
    [SerializeField] private Camera _photoCamera;
    [SerializeField] private Image _image;


    private void Update()
    {
        PhotoTarget target = _photoTargetDetector.MostCenterTarget;
        if (!target)
        {
            _image.enabled = false;
            return;
        }
        else
            _image.enabled = true;

        Vector2 screenPos = _photoCamera.WorldToScreenPoint(target.transform.position);
        RectTransform rect = _image.rectTransform;
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, screenPos, Time.deltaTime * 10f);
    }


}