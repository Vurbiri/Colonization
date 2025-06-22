using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.UI
{
    sealed public class PortButton : AEdificeButton
    {
        private const int MAX_HEX = 2;

        [Space]
        [SerializeField] private TextMeshProUGUI[] _hexIds;

        public override void Init(Crossroad crossroad, InputController inputController, int index, Sprite sprite, bool isOn)
        {
            base.Init(crossroad, inputController, index, sprite, isOn);

            int hexCount = 0;
            List<Hexagon> hexagons = crossroad.Hexagons;
            
            for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                if (hexagons[i].IsWater)
                    _hexIds[hexCount++].text = hexagons[i].ID.ToString();
            
            for (int i = hexCount; i < MAX_HEX; i++)
                Destroy(_hexIds[i].gameObject);
        }

        public override void OnChange(Crossroad crossroad, Sprite sprite)
        {
            _icon.sprite = sprite;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if(_hexIds == null || _hexIds.Length != MAX_HEX) 
                _hexIds = GetComponentsInChildren<TextMeshProUGUI>();
        }
#endif
    }
}
