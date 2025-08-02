using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Alchemy.Inspector;

public class PhotoCameraDetector : MonoBehaviour
{
    [Header("Configuration")] public Camera _photoCamera; // Caméra qui rend dans une RenderTexture
    public RenderTexture _renderTexture; // RenderTexture utilisée
    public LayerMask _specialLayerName;
    public float _maxDistance = 50f;

    [Header("Sauvegarde")] [Tooltip("Chemin complet de sauvegarde. Exemple : C:/Users/TonNom/Documents/MyScreenshots")]
    public string _saveFolderPath = ""; // À définir dans l’inspector

    public List<Texture2D> _sprites = new List<Texture2D>();
    public int _limit = 10;
    private int index = 0;

    [Button]
    public void PrintPhotos()
    {
        StartCoroutine(SavePhotosCoroutine());
    }

    private IEnumerator SavePhotosCoroutine()
    {
        if (string.IsNullOrEmpty(_saveFolderPath))
        {
            _saveFolderPath = Application.dataPath;
        }

        if (!Directory.Exists(_saveFolderPath))
        {
            Directory.CreateDirectory(_saveFolderPath);
        }

        for (int i = 0; i < _sprites.Count; i++)
        {
            Texture2D tex = _sprites[i];
            byte[] bytes = tex.EncodeToPNG(); // Encode synchroniquement (obligatoire)

            string fileName = $"SavedPhoto_{i + 1}.png";
            string filePath = Path.Combine(_saveFolderPath, fileName);

            // Écriture disque asynchrone, on attend que ça finisse sans bloquer le main thread
            Task writeTask = File.WriteAllBytesAsync(filePath, bytes);
            while (!writeTask.IsCompleted)
            {
                yield return new WaitForSeconds(1); // attend la fin de la sauvegarde sans freeze
            }

            Debug.Log($"💾 Photo {i + 1} sauvegardée : {filePath}");

            yield return new WaitForSeconds(1); // Découpe le traitement frame par frame pour éviter le freeze
        }

        if (Application.isEditor)
            AssetDatabase.Refresh();
    }

    public void TakePhoto()
    {
        //bool specialItemVisible = PerformBoxCast();

        AddSpriteToList();

        if (Application.isEditor)
            AssetDatabase.Refresh();
    }

    private void AddSpriteToList()
    {
        RenderTexture.active = _renderTexture;

        Texture2D image = new(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
        image.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        image.Apply();

        if (_sprites.Count < _limit)
        {
            _sprites.Add(image);
        }
        else
        {
            _sprites[index] = image;
            index++;
            index %= _limit;
        }
    }

    /*private bool PerformBoxCast()
    {
        float frustumHeight = 2.0f * _maxDistance * Mathf.Tan(_photoCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * _photoCamera.aspect;

        Vector3 halfExtents = new Vector3(frustumWidth / 2f, frustumHeight / 2f, 1);
        Vector3 origin = _photoCamera.transform.position + _photoCamera.transform.forward * (halfExtents.z / 2f);
        Vector3 direction = _photoCamera.transform.forward;

        return Physics.BoxCast(origin, halfExtents, direction, out RaycastHit hit, _photoCamera.transform.rotation,
            _maxDistance,
            _specialLayerName);
    }*/
}