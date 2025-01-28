using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[OrderInfo("Scenes",
              "LoadScene",
              "Loads a scene based on a given scene index or exact name")]
[AddComponentMenu("")]
public class LoadScene : Order
{
    [Tooltip("the exact name of the target level")]
    [SerializeField] protected string levelName;
    [Tooltip("the index of the target level")]
    [SerializeField] protected int levelIndex;
    [Tooltip("whether to load the scene asynchronously and wait to continue")]
    [SerializeField] protected bool loadAsync;
    public override void OnEnter()
    {
        if (loadAsync)
        {
            StartCoroutine(LoadSceneAsync());
        }
        else
        {
            LoadSceneNormal();
        }
    }

    private void LoadSceneNormal()
    {
        if (string.IsNullOrEmpty(levelName))
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        var asyncLoad = string.IsNullOrEmpty(levelName) ? SceneManager.LoadSceneAsync(levelIndex) : SceneManager.LoadSceneAsync(levelName);
        if (asyncLoad == null) { yield break; }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public override string GetSummary()
      => $"{(loadAsync ? "Asynchronously l" : "L")}oading Scene {(!string.IsNullOrEmpty(levelName) ? $"\"{levelName}\"" : $"Index: {levelIndex}")}.";
}