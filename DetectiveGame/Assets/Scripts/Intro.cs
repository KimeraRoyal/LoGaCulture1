using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    private Popups.Popup m_popup;

    [SerializeField] private string m_popupTitle;
    [TextArea(3, 10)] [SerializeField] private string m_popupDescription;

    [SerializeField] private string m_sceneName = "Menu";

    private void Awake()
    {
        m_popup = FindAnyObjectByType<Popups.Popup>();
    }

    private void Start()
    {
        m_popup.Show(m_popupTitle, null, m_popupDescription);
        m_popup.OnClosed += OnPopupClosed;
    }

    private void OnPopupClosed()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync(m_sceneName);
    }
}
