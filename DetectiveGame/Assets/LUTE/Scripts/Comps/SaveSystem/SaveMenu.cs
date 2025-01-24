using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    [Tooltip("String used to store save games in player prefs. Use unique strings for each game.")]
    [SerializeField] protected string saveKey = LogaConstants.DefaultSaveDataKey;
    [Tooltip("Auto load the most recent save when entering play")]
    [SerializeField] protected bool loadOnStart = false;
    [Tooltip("Auto save the game after each Save Point Order is executed")]
    [SerializeField] protected bool autoSave = true;
    [Tooltip("Show all options in the save menu - if false, disables to use of player saving")]
    [SerializeField] protected bool showAllOptions = true;
    [Tooltip("Delete save game data from disk when game is restarted - useful for debugging")]
    [SerializeField] protected bool deleteOnRestart = false;
    [SerializeField] protected CanvasGroup saveMenuGroup;
    [SerializeField] protected Button saveMenuButton;
    [SerializeField] protected Button saveButton;
    [SerializeField] protected Button loadButton;
    [SerializeField] protected Button rewindButton;
    [SerializeField] protected Button forwardButton;
    [SerializeField] protected Button restartButton;
    [SerializeField] protected ScrollRect debugView;

    protected static bool saveMenuActive = false;
    protected AudioSource menuAudioSource;
    protected LTDescr fadeTween; //Used for fading menu
    protected SaveMenu instance; //Used for singleton
    protected static bool hasLoadedOnStart; //Used to prevent multiple loads on start

    protected virtual void Awake()
    {/*
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        if (transform.parent == null)
        {
            GameObject.DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogError("SaveMenu should be a root object in the scene hierarchy otherwise it cannot be preserved across scenes.");
        }*/

        menuAudioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        if (!saveMenuActive)
        {
            saveMenuGroup.alpha = 0f;
        }

        var saveManager = LogaManager.Instance.SaveManager;

        if (string.IsNullOrEmpty(saveManager.StartScene))
        {
            saveManager.StartScene = SceneManager.GetActiveScene().name;
        }

        if (loadOnStart && !hasLoadedOnStart)
        {
            hasLoadedOnStart = true;

            if (saveManager.HasSaveData(saveKey))
            {
                saveManager.Load(saveKey);
            }
        }
    }

    public void ToggleSaveMenu()
    {
        if (fadeTween != null)
        {
            LeanTween.cancel(fadeTween.id, true);
            fadeTween = null;
        }

        if (saveMenuActive)
        {
            //Fade menu out
            LeanTween.value(saveMenuGroup.gameObject, saveMenuGroup.alpha, 0f, 0.4f)
    .setEase(LeanTweenType.easeOutQuint)
    .setOnUpdate((t) =>
    {
        saveMenuGroup.alpha = t;
    }).setOnComplete(() =>
    {
        saveMenuGroup.alpha = 0f;
    });
        }
        else
        {
            //Fade menu in
            LeanTween.value(saveMenuGroup.gameObject, saveMenuGroup.alpha, 1f, 0.4f)
    .setEase(LeanTweenType.easeOutQuint)
    .setOnUpdate((t) =>
    {
        saveMenuGroup.alpha = t;
    }).setOnComplete(() =>
    {
        saveMenuGroup.alpha = 1f;
    });
        }
        saveMenuActive = !saveMenuActive;
    }
}
