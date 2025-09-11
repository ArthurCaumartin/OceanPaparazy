using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using UnityEditor.Rendering;

//TODO : a lire pour plus tard https://discussions.unity.com/t/create-png-at-runtime-from-all-active-sprites-in-a-scene/938146/2
//? first freestyle un truc en speed

[RequireComponent(typeof(Camera))]
public class CameraPhotoTaker : MonoBehaviour
{
    [SerializeField] private RenderTexture _renderTexture;
    private string _saveFolderPath;
    private Coroutine photoCoroutine;

    private void Awake()
    {

        _saveFolderPath = Path.Combine(Application.dataPath, "Photos", "RuntimePhotos");
        if (!Directory.Exists(_saveFolderPath))
        {
            Directory.CreateDirectory(_saveFolderPath);
        }
    }

    public void TakePhoto(PhotoTarget target = null)
    {
        if (photoCoroutine != null)
            return;
        photoCoroutine = StartCoroutine(SavePNG(target));
    }

    //? Texture2D.ReadPixels dois etre call apres que la target texture ai ete rendu
    private IEnumerator SavePNG(PhotoTarget target = null)
    {
        yield return new WaitForEndOfFrame();
        Stopwatch stopwatch = Stopwatch.StartNew();

        string targetName = target ? target.name : "NoTarget";
        byte[] bytes = GetTextureBytes(_renderTexture);
        string filePath = GetPathToSave(targetName);

        File.WriteAllBytesAsync(filePath, bytes);
        target?.MarkPhotoAsTaken(filePath);

        if (Application.isEditor)
            AssetDatabase.Refresh();

        photoCoroutine = null;
        stopwatch.Stop();
        print($"Photo taken in {stopwatch.ElapsedMilliseconds} ms // Save to {filePath}");
    }

    private byte[] GetTextureBytes(RenderTexture texture)
    {
        Texture2D tex = new(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture.active = texture;
        tex.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        RenderTexture.active = null;
        tex.Apply();
        return tex.EncodeToPNG();
    }

    private string GetPathToSave(string fileName)
    {
        return Path.Combine(_saveFolderPath, $"TargetPhoto_{fileName}.png");
    }
}
