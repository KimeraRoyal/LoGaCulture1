using System;
using LUTE.EventHandlers;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HandledButton : MonoBehaviour
{
    private Button m_button;

    public Action OnPressed;

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnButtonClick);
        
        FindAnyObjectByType<HandledButtons>().RegisterButton(this);
    }

    private void OnButtonClick()
        => OnPressed?.Invoke();
}
