using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string _sceneNameHub = "Scene_Hub";
    [SerializeField] private string _sceneNameGame = "Scene_Game";

    public string SceneNameHub => _sceneNameHub;
    public string SceneNameGame => _sceneNameGame;

    public void LoadLevel(string sceneName, Action onLoaded = null)
    {
        StartCoroutine(LoadCoroutine(sceneName, onLoaded));
    }

    private IEnumerator LoadCoroutine(string sceneName, Action onLoaded = null)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        onLoaded?.Invoke();
    }
}

