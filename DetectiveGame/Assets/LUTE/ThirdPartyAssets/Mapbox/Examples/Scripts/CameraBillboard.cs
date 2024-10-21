using TMPro;

namespace Mapbox.Examples
{
    using UnityEngine;
    using UnityEngine.UI;

    public class CameraBillboard : MonoBehaviour
    {
        Camera _camera;
        Canvas canvas;
        Image image;
        [SerializeField] private TMP_Text label;
        MeshRenderer meshRenderer;
        public SpriteRenderer spriteRenderer;

        private bool showName = true;

        public TMP_Text Label
        {
            get
            {
                if (!label) { label = GetComponent<TMP_Text>(); }
                return label;
            }
        }
        
        void Awake()
        {
            canvas = GetComponentInChildren<Canvas>();
            image = GetComponentInChildren<Image>();
        }

        void Update()
        {
            if (_camera != null)
            {
                transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
            }
        }

        public void SetCanvasCam(Camera cam)
        {
            _camera = cam;
            if (canvas == null)
                canvas = GetComponentInChildren<Canvas>();
            if (canvas != null)
                canvas.worldCamera = cam;
        }

        public Camera GetCurrentCam()
        {
            if (canvas == null)
                canvas = GetComponentInChildren<Canvas>();
            if (canvas != null)
                return canvas.worldCamera;

            return null;
        }

        public RectTransform GetImageTrans()
        {
            if (image == null)
                image = GetComponentInChildren<Image>();
            return image.GetComponent<RectTransform>();
        }

        public void SetText(string text)
        {
            if(!Label) { return; }
            Label.text = text;
        }

        public void SetColor(Color color)
        {
            if(!Label) { return; }
            Label.color = color;
        }

        public void SetName(bool show)
        {
            if(!Label) { return; }
            
            showName = show;
            Label.text = showName ? Label.text : "";
        }

        public void SetIcon(Sprite icon)
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.sprite = icon;
        }
    }
}