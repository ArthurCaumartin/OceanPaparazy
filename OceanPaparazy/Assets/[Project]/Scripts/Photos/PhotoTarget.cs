using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
    [SerializeField] private ScriptablePhotoTarget scriptablePhotoTarget;

    public string Name
    {
        get { return scriptablePhotoTarget ? scriptablePhotoTarget.Name : "No Data Set"; }
    }

    public bool IsPhotoTaken
    {
        get { return scriptablePhotoTarget ? scriptablePhotoTarget.IsPhotoTaken : false; }
    }

    public void MarkPhotoAsTaken(string photoPath)
    {
        if (!scriptablePhotoTarget)
        {
            Debug.LogWarning("ScriptablePhotoTarget is not assigned.");
            return;
        }

        scriptablePhotoTarget.IsPhotoTaken = true;
        scriptablePhotoTarget.PhotoPath = photoPath;
    }
}
