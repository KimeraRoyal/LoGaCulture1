using KR;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButton : MonoBehaviour
{
    private LoadingScreen m_loadingScreen;

    private Button m_button;
    
    private void Awake()
    {
        m_loadingScreen = FindAnyObjectByType<LoadingScreen>();

        m_button = GetComponent<Button>();
        
        m_button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        m_loadingScreen.OnOpened += OnLoadingScreenOpened;
    }

    private void OnLoadingScreenOpened()
    {
        m_loadingScreen.OnOpened -= OnLoadingScreenOpened;
        SceneManager.LoadSceneAsync("Game");
    }
}
