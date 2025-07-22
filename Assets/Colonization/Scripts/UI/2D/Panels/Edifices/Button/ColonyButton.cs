using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    sealed public class ColonyButton : AEdificeButton
    {
        [Space]
        [SerializeField] private TextMeshProUGUI[] _hexIds;
        [SerializeField] private GameObject _wall;

        public override void Init(Crossroad crossroad, int index, Sprite sprite, bool isOn)
        {
            base.Init(crossroad, index, sprite, isOn);

            List<Hexagon> hexagons = crossroad.Hexagons;
            for (int i = 0; i < Crossroad.HEX_COUNT; i++)
            {
                int temp = i;
                hexagons[i].Subscribe(id => _hexIds[temp].text = id.ToString());
            }

            _wall.SetActive(crossroad.IsWall);
        }

        public override void OnChange(Crossroad crossroad, Sprite sprite)
        {
            _icon.sprite = sprite;
            _wall.SetActive(crossroad.IsWall);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_hexIds == null || _hexIds.Length != Crossroad.HEX_COUNT)
                _hexIds = GetComponentsInChildren<TextMeshProUGUI>();
            if (_wall == null)
                _wall = EUtility.GetComponentInChildren<RectTransform>(this, "Wall").gameObject;
        }
#endif
    }
}
