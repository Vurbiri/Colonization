//Assets\Colonization\Scripts\UI\_UIGame\Panels\Editor\PlayerPanels_Editor.cs
#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public partial class PlayerPanels : ICanvasElement
    {
        [Header("┌──────────── Editor ─────────────────────")]
        [SerializeField, ReadOnly] private ColorSettingsScriptable _colorSettings;
        [Space]
        [SerializeField, Range(1f, 3f)] private float _pixelsPerUnit = 1.5f;
        [Header("Panels In")]
        [SerializeField] private Vector2 _paddingIn = new(14f, 12f);
        [SerializeField, Range(1f, 10f)] private float _spaceIn = 4f;
        [Header("Between")]
        [SerializeField] private Vector2 _paddingOut = new(15f, 15f);
        [SerializeField, Range(1f, 20f)] private float _spaceOut = 7f;
        [SerializeField, Range(0.1f, 1f)] private float _advRatioSpaceOut = 0.4f;
        [Header("└────────────────────────────────────")]
        #pragma warning disable 414
        [SerializeField, ReadOnly] private string _endEditor = "****************************************************************";
        #pragma warning restore 414

        public void UpdateVisuals()
        {
            var colors = _colorSettings.Colors;
            var advPadding = _spaceOut * _advRatioSpaceOut;

            RectTransform thisRectTransform = (RectTransform)transform;

            List<RectTransform> rectEdifices = new(EdificeGroupId.Count);
            for (int i = EdificeGroupId.Count - 1; i >= 0; i--)
                rectEdifices.Add(_edifices[i].UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors));

            RectTransform rectRoads = _roads.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors);
            RectTransform rectWarriors = _warriors.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors);
            RectTransform rectCurrencies = _currencies.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, _spaceIn, colors);
            RectTransform rectBlood = _blood.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors);

            //=======

            Vector3 position = -thisRectTransform.rect.size * 0.5f + _paddingOut;

            rectEdifices[0].localPosition = position;
            for (int i = 1; i < EdificeGroupId.Count; i++)
                rectEdifices[i].localPosition = position = NextPosition(position, rectEdifices[i - 1]);
            rectRoads.localPosition = position = NextPosition(position, rectEdifices[EdificeGroupId.Count - 1], advPadding * .7f);

            rectWarriors.localPosition = position = NextPosition(position, rectRoads, advPadding);

            rectCurrencies.localPosition = position = NextPosition(position, rectWarriors, advPadding);
            rectBlood.localPosition =  NextPosition(position, rectCurrencies);

            // Local function
            Vector3 NextPosition(Vector3 current, RectTransform prevPanel, float advOffset = 0f)
            {
                current.x += prevPanel.rect.width + _spaceOut + advOffset;
                return current;
            }
        }

        public void Rebuild(CanvasUpdate executing)
        {
            if (executing == CanvasUpdate.Prelayout && !Application.isPlaying)
                UpdateVisuals();
        }
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        public bool IsDestroyed() => this == null;

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                EUtility.SetScriptable(ref _colorSettings);

                EUtility.SetObject(ref _roads);
                EUtility.SetObject(ref _warriors);
                EUtility.SetObject(ref _currencies);
                EUtility.SetObject(ref _blood);

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);

                for (int i = 0; i < EdificeGroupId.Count; i++)
                {
                    if (_edifices[i] == null)
                        _edifices[i] = EUtility.FindObjectByName<AEdificesPanel>($"{EdificeGroupId.GetName(i)}Panel");
                }

                for (int i = 0; i < EdificeId.Count; i++)
                {
                    if (i > 0 && _sprites[i] == null)
                        _sprites[i] = EUtility.FindAnyAsset<Sprite>($"SP_Icon{EdificeId.GetName(i)}");
                }
            }
        }
    }
}

#endif
