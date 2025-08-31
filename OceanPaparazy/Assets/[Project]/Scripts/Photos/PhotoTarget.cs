using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
    [SerializeField] private string targetName;
    [SerializeField] private int scoreValue;

    public string TargetName => targetName;
    public int ScoreValue => scoreValue;

}
