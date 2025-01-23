using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private SavePrefs m_save;
    private LoadPrefs m_load;

    private static SaveSystem s_instance;

    private void Awake()
    {
        if(s_instance != null && s_instance != this) { Destroy(gameObject); }
        
        DontDestroyOnLoad(gameObject);
        s_instance = this;
    }
}
