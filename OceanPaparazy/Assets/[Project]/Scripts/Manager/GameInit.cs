using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour
{
    [SerializeField] private string _sceneNameManager = "Scene_Manager";


    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != _sceneNameManager)
        {
            SceneManager.LoadScene(_sceneNameManager, LoadSceneMode.Additive);
        }
    }
}

