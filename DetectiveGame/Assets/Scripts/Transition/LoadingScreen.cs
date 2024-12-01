using System;
using UnityEngine;

namespace KR
{
    [RequireComponent(typeof(Openable))]
    public class LoadingScreen : MonoBehaviour
    {
        private Openable m_openable;
        
        public Action OnOpened;
        public Action OnClosed;

        private void Awake()
        {
            m_openable = GetComponent<Openable>();

            var loadingScreens = FindObjectsOfType<LoadingScreen>();
            if(loadingScreens.Length > 1) { Destroy(gameObject); }
            
            DontDestroyOnLoad(gameObject);
        }

        public void Open()
            => m_openable.Open();

        public void Close()
            => m_openable.Close();

        private void Opened()
            => OnOpened?.Invoke();

        private void Closed()
            => OnClosed?.Invoke();
    }
}
