//Assets\Colonization\Editor\Currencies\Scripts\CurrencyIcon.cs
using System;
using UnityEngine;

namespace VurbiriEditor.Colonization
{
    [Serializable]
    public class CurrencyIcon
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _color = Color.white;

        public Sprite Icon => _icon;
        public Color Color => _color;
    }
}
