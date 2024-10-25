using UnityEngine;

namespace KR.Elements.Camera
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasCamera : MonoBehaviour, ICameraElement
    {
        private Canvas m_canvas;

        private void Awake()
            => m_canvas = GetComponent<Canvas>();

        public UnityEngine.Camera GetCamera()
            => m_canvas.worldCamera;

        public void SetCamera(UnityEngine.Camera _camera)
            => m_canvas.worldCamera = _camera;
    }
}