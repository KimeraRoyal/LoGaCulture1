using UnityEngine;

namespace KR.TopRow
{
    public class BackButton : MonoBehaviour
    {
        private TopRow m_topRow;

        [SerializeField] private Sprite m_icon;
        
        private void Awake()
        {
            m_topRow = GetComponent<TopRow>();
        }

        private void Start()
        {
            var popupIcon = PopupIcons.GetPopupIcons();
            popupIcon.SetActive(true);
            
            popupIcon.SetIcon(m_icon);
            popupIcon.SetAction(m_topRow.Close);
            
            popupIcon.MoveToNextOption();
        }
    }
}