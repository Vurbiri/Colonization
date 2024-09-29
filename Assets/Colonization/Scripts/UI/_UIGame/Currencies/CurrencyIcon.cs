using System;
using UnityEngine;

namespace Vurbiri.Colonization.UI
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
