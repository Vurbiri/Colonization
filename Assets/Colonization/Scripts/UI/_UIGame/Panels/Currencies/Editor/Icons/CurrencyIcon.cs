//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\Editor\Icons\CurrencyIcon.cs
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    [Serializable]
    public class CurrencyIcon
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _color = Color.white;

        public Sprite Icon => _icon;
        public Color Color => _color;

        public void ToImage(Image image)
        {
            image.sprite = _icon;
            image.color = _color;
        }
    }
}
