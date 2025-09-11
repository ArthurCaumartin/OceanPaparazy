using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "🐠🐟 OceanPaparazy 🦈🐡/Album")]
public class ScribtableAlbum : ScriptableObject
{
    public List<ScriptablePhotoTarget> photoTargetList;

    public void GetcompletedValues(out int takenPhotos, out int totalPhotos)
    {
        takenPhotos = 0;
        totalPhotos = photoTargetList.Count;

        foreach (var photoTarget in photoTargetList)
        {
            if (photoTarget.IsPhotoTaken)
                takenPhotos++;
        }
    }

    public bool IsAlbumComplete
    {
        get
        {
            foreach (var photoTarget in photoTargetList)
            {
                if (!photoTarget.IsPhotoTaken)
                    return false;
            }
            return true;
        }
    }
}
