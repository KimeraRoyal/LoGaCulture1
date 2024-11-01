using System;
using UnityEngine;

namespace IP3.Interaction.Click
{
    public class Clicker : MonoBehaviour
    {
        private enum RayType
        {
            Ray2D,
            Ray3D
        }
        
        [SerializeField] private Camera m_camera;
        [SerializeField] private RayType m_rayType = RayType.Ray2D;

        [SerializeField] private LayerMask m_mask;
        [SerializeField] private bool m_hold;
        
        private Clickable m_currentClicked;

        private int m_blocking;

        public bool Blocked => m_blocking > 0;

        public Action<Clickable> OnClick;

        public Action<Clickable> OnHold;
        public Action<Clickable> OnRelease;

        private void Awake()
        {
            if (!m_camera) { m_camera = GetComponent<Camera>(); }
        }

        private void OnDisable()
        {
            Release();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Blocked) { return; }
                
                var clicked = Click();
                if (m_hold && clicked)
                {
                    m_currentClicked = clicked;
                    OnHold?.Invoke(clicked);
                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
                Release();
            }
        }

        private Clickable Click()
        {
            Clickable clickable = null;
            if (ShootRay(out var _collider))
            {
                clickable = _collider.GetComponentInParent<Clickable>();
                if (clickable) { clickable.Click(m_hold); }
            }
            OnClick?.Invoke(clickable);
            
            return clickable;
        }

        private void Release()
        {
            if(!m_currentClicked) { return; }

            m_currentClicked.Release();
            m_currentClicked = null;
            
            OnRelease?.Invoke(m_currentClicked);
        }

        private bool ShootRay(out Transform o_colliderTransform)
        {
            o_colliderTransform = null;
            
            switch (m_rayType)
            {
                case RayType.Ray2D:
                {
                    if (ShootRay2D(out var _rayHit))
                    {
                        o_colliderTransform = _rayHit.collider.transform;
                        return true;
                    }
                    break;
                }
                case RayType.Ray3D:
                {
                    if (ShootRay3D(out var _rayHit))
                    {
                        Debug.Log("Hit!");
                        o_colliderTransform = _rayHit.collider.transform;
                        return true;
                    }
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            
            return false;
        }

        private bool ShootRay2D(out RaycastHit2D o_rayHit)
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            o_rayHit = Physics2D.Raycast(ray.origin, Vector2.up, 0.001f, m_mask);
            return o_rayHit.collider;
        }

        private bool ShootRay3D(out RaycastHit o_rayHit)
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 1000.0f, Color.red, 2.0f);
            return Physics.Raycast(ray, out o_rayHit, m_camera.farClipPlane, m_mask);
        }

        public void Block()
            => m_blocking++;

        public void Unblock()
            => m_blocking--;
    }
}
