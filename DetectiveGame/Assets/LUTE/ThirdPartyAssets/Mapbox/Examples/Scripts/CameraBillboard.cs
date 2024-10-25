using KR.Elements.Camera;
using UnityEngine;

namespace Mapbox.Examples
{
    public class CameraBillboard : MonoBehaviour, ICameraElement
    {
        private Camera m_camera;

        private bool showName = true;

        private void Update()
        {
            if (!m_camera) { return; }
            transform.LookAt(transform.position + m_camera.transform.rotation * Vector3.forward, m_camera.transform.rotation * Vector3.up);
        }

        public Camera GetCamera()
            => m_camera;

        public void SetCamera(Camera _camera)
            => m_camera = _camera;
    }
}