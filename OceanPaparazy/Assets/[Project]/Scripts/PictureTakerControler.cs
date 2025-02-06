using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PictureTakerControler : MonoBehaviour
{
    [SerializeField, Range(0.01f, 0.2f)] private float _rayResolutionX;
    [SerializeField, Range(0.01f, 0.2f)] private float _rayResolutionY;
    [SerializeField] private float _range = 100;
    [SerializeField] private Camera _camera;
    private int _count;

    private void Start()
    {
        _count = 0;
    }


    void Update()
    {
    }

    private void OnShoot(InputValue value)
    {
        if (!_camera) return;
        if (value.Get<float>() > .5f)
        {
            TakePicture();
            BurstRay();
        }
    }

    public void BurstRay()
    {
        Texture2D tex2D = new Texture2D((int)(1 / _rayResolutionX), (int)(1 / _rayResolutionY));
        for (float x = 0; x < 1; x += _rayResolutionX)
        {
            for (float y = 0; y < 1; y += _rayResolutionY)
            {
                Vector2 pixelPos;
                pixelPos.x = Mathf.Lerp(0, _camera.pixelWidth, x);
                pixelPos.y = Mathf.Lerp(0, _camera.pixelHeight, y);
                Ray r = _camera.ScreenPointToRay(pixelPos);
                Physics.Raycast(r, out RaycastHit hit, _range);

                if (hit.collider)
                {
                    Debug.DrawLine(hit.point + Vector3.down, hit.point + Vector3.up, Color.green);
                    // float depth = Vector3.Distance(_camera.transform.position, hit.point);
                    // depth = Mathf.InverseLerp(0, 150, depth);
                    tex2D.SetPixel((int)Mathf.Lerp(0, tex2D.width, x)
                                    , (int)Mathf.Lerp(0, tex2D.height, y)
                                    // , Color.Lerp(Color.cyan, Color.black, depth));
                                    , Color.cyan);
                }
                else
                {
                    tex2D.SetPixel((int)Mathf.Lerp(0, tex2D.width, x), (int)Mathf.Lerp(0, tex2D.height, y), Color.black);
                }

                Debug.DrawRay(r.origin, r.direction * _range, new Color(1, 0, 0, .4f));

            }
        }

        tex2D.Apply();
        SavePNG("RayTexture", tex2D);
    }

    public void TakePicture()
    {
        RenderTexture renderTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 24);
        _camera.targetTexture = renderTexture;
        _camera.Render();

        Texture2D texture = new Texture2D(_camera.pixelWidth, _camera.pixelHeight, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, _camera.pixelWidth, _camera.pixelHeight), 0, 0);
        texture.Apply();

        SavePNG("Photo", texture);

        _camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        Destroy(texture);
    }


    public void SavePNG(string pngName, Texture2D tex)
    {
        byte[] bytes = tex.EncodeToPNG();
        string filePath = Path.Combine(Application.dataPath + "/[Project]/Photos", $"{pngName}_{_count}.png");
        File.WriteAllBytes(filePath, bytes);
        if (Application.isEditor)
            AssetDatabase.Refresh();

        _count++;
    }
}
