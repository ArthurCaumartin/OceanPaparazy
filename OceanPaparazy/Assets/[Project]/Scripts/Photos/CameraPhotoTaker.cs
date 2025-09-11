using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

//TODO : a lire pour plus tard https://discussions.unity.com/t/create-png-at-runtime-from-all-active-sprites-in-a-scene/938146/2
//? first freestyle un truc en speed

[RequireComponent(typeof(Camera))]
public class CameraPhotoTaker : MonoBehaviour
{
    [SerializeField] private List<PhotoData> _photoDataList = new List<PhotoData>();
    private RenderTexture _renderTexture;
    private Camera _photoCamera;
    private string _saveFolderPath;

    private void Awake()
    {
        _photoCamera = GetComponent<Camera>();
        _renderTexture = _photoCamera.targetTexture;

        _saveFolderPath = Path.Combine(Application.dataPath, "Photos", "RuntimePhotos");
        if (!Directory.Exists(_saveFolderPath))
        {
            Directory.CreateDirectory(_saveFolderPath);
        }
    }

    public void SavePNG(PhotoTarget target = null)
    {
        string targetName = target ? target.name : "NoTarget";

        Texture2D tex = new(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();

        string fileName = $"SavedPhoto_{targetName}.png";
        string filePath = Path.Combine(_saveFolderPath, fileName);
        File.WriteAllBytesAsync(filePath, bytes);
        Debug.Log($"💾 Photo sauvegardée : {filePath}");

        PhotoData photoData = new PhotoData
        {
            photoPath = filePath,
            entityName = targetName
        };
        _photoDataList.Add(photoData);

        if (Application.isEditor)
            AssetDatabase.Refresh();
    }
}
