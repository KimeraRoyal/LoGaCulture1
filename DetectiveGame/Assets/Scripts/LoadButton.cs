using KR;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadButton : MonoBehaviour
{
    private LoadingScreen m_loadingScreen;
    private LoadPrefs m_loadPrefs;

    private Button m_button;
    
    private void Awake()
    {
        m_loadingScreen = FindAnyObjectByType<LoadingScreen>();
        m_loadPrefs = FindAnyObjectByType<LoadPrefs>();

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
        m_loadPrefs.Load();
    }
}
