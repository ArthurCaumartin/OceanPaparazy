using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class PhotoCameraDetector : MonoBehaviour
{
    [Header("Configuration")] public Camera _photoCamera; // Caméra qui rend dans une RenderTexture
    public RenderTexture _renderTexture; // RenderTexture utilisée
    public LayerMask _specialLayerName;
    public float _maxDistance = 50f;

    [Header("Sauvegarde")] [Tooltip("Chemin complet de sauvegarde. Exemple : C:/Users/TonNom/Documents/MyScreenshots")]
    public string _saveFolderPath = ""; // À définir dans l’inspector
    
    public List<Texture2D> _sprites;
    
    public void TakePhoto()
    {
        bool specialItemVisible = PerformBoxCast();

        // Sauvegarde
        SaveRenderTextureToPNG(_renderTexture, specialItemVisible);

        // Log
        //Debug.Log(specialItemVisible
        //    ? "📸 Objet spécial visible dans la photo !"
        //    : "📸 Aucun objet spécial détecté.");

        if (Application.isEditor)
            AssetDatabase.Refresh();
    }

    private void SaveRenderTextureToPNG(RenderTexture rt, bool specialItemDetected)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D image = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        image.Apply();

        byte[] bytes = image.EncodeToPNG();
        Object.DestroyImmediate(image);

        // Vérifier et créer le dossier si nécessaire
        if (string.IsNullOrEmpty(_saveFolderPath))
        {
            _saveFolderPath =  Application.dataPath;
        }

        if (!Directory.Exists(_saveFolderPath))
        {
            Directory.CreateDirectory(_saveFolderPath);
        }

        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = specialItemDetected
            ? $"Photo_SPECIAL_{timestamp}.png"
            : $"Photo_{timestamp}.png";

        string filePath = Path.Combine(_saveFolderPath, fileName);
        File.WriteAllBytes(filePath, bytes);

        //Debug.Log($"💾 Photo sauvegardée : {filePath}");

        RenderTexture.active = currentRT;
    }

    private bool PerformBoxCast()
    {
        // Calculer la hauteur et largeur du frustum à maxDistance
        float frustumHeight = 2.0f * _maxDistance * Mathf.Tan(_photoCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * _photoCamera.aspect;

        // Half extents du box (la moitié de la taille)
        Vector3 halfExtents = new Vector3(frustumWidth / 2f, frustumHeight / 2f, 1);
        // On prend une petite épaisseur en profondeur (z) car on va faire un boxcast en ligne

        Vector3 origin = _photoCamera.transform.position + _photoCamera.transform.forward * (halfExtents.z / 2f);
        Vector3 direction = _photoCamera.transform.forward;
        
        // Lance le BoxCast
        return (Physics.BoxCast(origin, halfExtents, direction, out RaycastHit hit, _photoCamera.transform.rotation,
            _maxDistance,
            _specialLayerName));
    }
}