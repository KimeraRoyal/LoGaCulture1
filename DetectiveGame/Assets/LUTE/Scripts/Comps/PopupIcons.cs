using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupIcons : MonoBehaviour
{
    public static PopupIcons activePopupIcons { get; set; }

    private Popup popupWindow;
    private Button[] cachedButtons;
    private int nextOptionIndex;

    public virtual Button[] CachedButtons { get { return cachedButtons; } }

    private void Awake()
    {
        Debug.Log("Initialized");
        Button[] optionButtons = GetComponentsInChildren<Button>();
        cachedButtons = optionButtons;

        foreach (Button button in cachedButtons)
        {
            button.gameObject.SetActive(false);
        }

        if (cachedButtons.Length <= 0)
        {
            Debug.LogError("PopupIcon requires a Button component on a child object");
            return;
        }

        CheckEventSystem();
    }

    protected virtual void CheckEventSystem()
    {
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            // Auto spawn an Event System from the prefab - ensure you have one in a Resources folder
            GameObject prefab = Resources.Load<GameObject>("Prefabs/EventSystem");
            if (prefab != null)
            {
                GameObject go = Instantiate(prefab) as GameObject;
                go.name = "EventSystem";
            }
        }
    }

    public static PopupIcons GetPopupIcons()
    {
        if (!activePopupIcons) { activePopupIcons = FindAnyObjectByType<PopupIcons>(); } 
        return activePopupIcons;
    }

    public bool SetIcon(Sprite icon)
    {
        if (nextOptionIndex >= CachedButtons.Length)
        {
            Debug.LogWarning("Unable to add popup option, not enough buttons!");
            return false;
        }
        activePopupIcons.CachedButtons[nextOptionIndex].image.sprite = icon;
        return true;
    }

    public void SetPopupWindow(Popup popupWindow)
    {
        this.popupWindow = popupWindow;
    }

    private void OnClick()
    {
        if (popupWindow != null)
        {
            popupWindow.OpenClose();
        }
    }

    public bool SetAction(UnityAction onClick)
    {
        if (nextOptionIndex >= CachedButtons.Length)
        {
            Debug.LogWarning("Unable to add popup option, not enough buttons!");
            return false;
        }
        activePopupIcons.CachedButtons[nextOptionIndex].onClick.AddListener(() => { onClick.Invoke(); });
        activePopupIcons.CachedButtons[nextOptionIndex].gameObject.SetActive(true);
        return true;
    }

    public void MoveToNextOption()
    {
        nextOptionIndex++;
    }

    public virtual void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }
}
