﻿using UnityEngine;
using UnityEngine.UI;

namespace KR.Elements.Graphic
{
    [RequireComponent(typeof(Image))]
    public class ImageGraphic : MonoBehaviour, IGraphicElement
    {
        private Image m_image;

        private void Awake()
            => m_image = GetComponent<Image>();

        public UnityEngine.Sprite GetSprite()
            => m_image.sprite;

        public void SetSprite(UnityEngine.Sprite _sprite)
            => m_image.sprite = _sprite;
    }
}