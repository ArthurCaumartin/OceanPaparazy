using UnityEngine;

[CreateAssetMenu(menuName = "🐠🐟 OceanPaparazy 🦈🐡/PhotoTarget")]
public class ScriptablePhotoTarget : ScriptableObject
{
    [SerializeField] private string entityName;
    [SerializeField] private bool isPhotoTaken;
    [Space]
    [SerializeField] private Sprite drawingSketch;
    [SerializeField] private string photoPath;

    public string Name => entityName;
    public bool IsPhotoTaken { get => isPhotoTaken; set => isPhotoTaken = value; }
    public string PhotoPath { get => photoPath; set => photoPath = value; }
}